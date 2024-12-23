using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        int bomb_num;
        Button[] buttons = new Button[12];
        Label bomb_input = new Label();
        Label bomb_see = new Label();
        Label time_see = new Label();

        Button[] ground = new Button[120];
        int have_bomb,have_mark,have_flag,have_opened;
        int [] bomb_pos = new int[99];

        Timer time_trigger = new Timer();
        int time;
        Button flag = new Button();

        Label result = new Label();
        Button restart = new Button();

        /*form1*/
        public Form1()
        {
            InitializeComponent();
            this.Size = new Size(520, 350);
            this.BackColor = Color.White;

            for (int i = 0; i < 12; i++)
            {
                Button tem = new Button();
                tem.Location = new Point(180 + 45 * (i % 3), 75 + 50 * (i / 3));
                tem.Size = new Size(40, 40);
                tem.Name = $"button{i}";
                tem.Tag = i;
                tem.Text = $"{i}";
                tem.Click += new EventHandler(this.keyboard_Click);
                tem.BackColor = Color.GhostWhite;
                tem.Enabled = false;
                tem.Visible = false;
                Controls.Add(tem);
                buttons[i] = tem;
            }
            buttons[10].Text = $"C";
            buttons[11].Text = $"S";
            buttons[11].Click -= new EventHandler(this.keyboard_Click);
            buttons[11].Click += new EventHandler(this.Form2);

            for (int i = 0; i < 120; i++)
            {
                Button tem = new Button();
                tem.Location = new Point(40 + 28 * (i % 15), 70 + 28 * (i / 15));
                tem.Size = new Size(28, 28);
                tem.Name = $"ground{i}";
                tem.Tag = i;
                tem.Click += new EventHandler(this.ground_Click);
                tem.EnabledChanged += new EventHandler(this.count_open);
                tem.BackColor = Color.GhostWhite;
                tem.Enabled = false;
                tem.Visible = false;
                Controls.Add(tem);
                ground[i] = tem;
            }

            bomb_input.Location = new Point(180, 30);
            bomb_input.Size = new Size(130, 30);
            bomb_input.BackColor = Color.GhostWhite;
            bomb_input.TextAlign = ContentAlignment.MiddleRight;
            bomb_input.BorderStyle = BorderStyle.FixedSingle;
            bomb_input.Tag = 12;
            bomb_input.Visible = false;
            Controls.Add(bomb_input);

            bomb_see.Location = new Point(70, 20);
            bomb_see.Size = new Size(60, 30);
            bomb_see.BackColor = Color.GhostWhite;
            bomb_see.TextAlign = ContentAlignment.MiddleCenter;
            bomb_see.BorderStyle = BorderStyle.FixedSingle;
            bomb_see.Visible = false;
            Controls.Add(bomb_see);

            time_see.Location = new Point(330, 20);
            time_see.Size = new Size(100, 30);
            time_see.BackColor = Color.GhostWhite;
            time_see.TextAlign = ContentAlignment.MiddleCenter;
            time_see.BorderStyle = BorderStyle.FixedSingle;
            time_see.Visible = false;
            Controls.Add(time_see);

            time_trigger.Interval = 1000;
            time_trigger.Tick += new EventHandler(this.time_count);
            time_trigger.Enabled = false;

            flag.Location = new Point(250, 20);
            flag.Size = new Size(70, 30);
            flag.Click += new EventHandler(this.flag_Click);
            flag.BackColor = Color.GhostWhite;
            flag.Visible = false;
            flag.Enabled = false;
            Controls.Add(flag);

            result.Location = new Point(140, 20);
            result.Size = new Size(100, 30);
            result.BackColor = Color.GhostWhite;
            result.TextAlign = ContentAlignment.MiddleCenter;
            result.BorderStyle = BorderStyle.FixedSingle;
            result.Visible = false;
            Controls.Add(result);

            restart.Location = new Point(250, 20);
            restart.Size = new Size(70, 30);
            restart.Text = $"Restart";
            restart.Click += new EventHandler(this.restart_Click);
            restart.BackColor = Color.GhostWhite;
            restart.Enabled = false;
            restart.Visible = false;
            Controls.Add(restart);

            Form1_load();
        }
        private void Form1_load()
        {
            bomb_num = 5;
            for (int i = 0; i < 12; i++)
            {
                buttons[i].Enabled = true;
                buttons[i].Visible = true;
            }
            bomb_input.Visible = true;
            bomb_input.Text = Convert.ToString(bomb_num);
        }
        private void keyboard_Click(object sender, EventArgs e)
        {
            Button B_click = (Button)sender;
            int tem = Convert.ToInt16(B_click.Tag);
            if (tem < 10 && bomb_num == 0) bomb_num = tem;
            else if (tem < 10) bomb_num = bomb_num * 10 + tem;
            else if (tem == 10) bomb_num = 0;

            bomb_input.Text = Convert.ToString(bomb_num);
        }
        /*form2*/
        public void Form2(object sebder, EventArgs e)
        {
            if (bomb_num < 5) bomb_num = 5;
            else if (bomb_num > 99) bomb_num = 99;

            for (int i = 0; i < 12; i++)
            {
                buttons[i].Enabled = false;
                buttons[i].Visible = false;
            }
            bomb_input.Visible = false;

            for (int i = 0; i < 120; i++)
            {
                ground[i].Enabled = true;
                ground[i].Visible = true;
                ground[i].BackColor = Color.GhostWhite;
                ground[i].Text = $"";
            }
            bomb_see.Visible = true;
            bomb_see.Text = Convert.ToString(bomb_num);

            time_see.Text = $"00 : 00";
            time_see.Visible = true;
            time = 0;

            Random ran = new Random();
            bomb_pos[0] = ran.Next(120);
            //ground[bomb_pos[0]].BackColor = Color.BlueViolet;
            for (int i = 1; i < bomb_num; i++)
            {
                bomb_pos[i] = 120;
                int tem = ran.Next(120);
                for(int j = 0; j < i; j++)
                {
                    if (bomb_pos[j] == tem)
                    {
                        j = i; i--;
                    }
                    else if (j == i - 1) bomb_pos[i] = tem;
                }
            //    ground[bomb_pos[i]].BackColor = Color.BlueViolet;
            }

            flag.Enabled = true;
            flag.Visible = true;
            flag.BackColor = Color.GhostWhite;

            have_opened = 0;
            have_mark = 0;
            have_flag = bomb_num;
            flag.Text = $"Flag: {have_flag}";

            time_trigger.Enabled = true;
        }
        private void time_count(object sender, EventArgs e)
        {
            time++;
            time_see.Text = $"{time/60/10}{time/60%10} : {time%60/10}{time%60%10}";
        }
        /*game*/
        private void ground_Click(object sender, EventArgs e)
        {
            Button tem = (Button)sender;
            int pos = Convert.ToInt16(tem.Tag);
            have_bomb = 0;
            Button B_click = (Button)sender;
            for (int i = 0; i < bomb_num; i++)
            {
                if (pos == bomb_pos[i])
                {
                    if (flag.BackColor == Color.GhostWhite 
                        && B_click.BackColor != Color.LightYellow)
                    {
                        openall(0);
                        gameover();
                        result.Text = $"Fail";
                        return;
                    }
                    else if (flag.BackColor == Color.LightYellow
                        && B_click.BackColor == Color.LightYellow)
                    {
                        have_mark--;
                       // B_click.BackColor = Color.GhostWhite;
                    }
                    else if (flag.BackColor == Color.LightYellow)
                    {
                        have_mark++;
                        //B_click.BackColor = Color.LightYellow;
                    }
                    i = bomb_num;
                }
                else have_bomb += Check_bomb(pos, bomb_pos[i]);
            }
            if (flag.BackColor == Color.GhostWhite)
            {
                if (B_click.BackColor == Color.GhostWhite)
                {
                    if (have_bomb > 0)
                    {
                        B_click.Enabled = false;
                        B_click.Text = Convert.ToString(have_bomb);
                    }
                    else
                    {
                        B_click.Enabled = false;
                        if (pos - 15 >= 0) open(pos - 15);
                        if (pos + 15 < 120) open(pos + 15);
                        if (pos % 15 != 0) open(pos - 1);
                        if (pos % 15 != 14) open(pos + 1);
                    }
                }
            }
            else if(B_click.BackColor == Color.LightYellow)
            {
                have_flag++;
                B_click.BackColor = Color.GhostWhite;
            }
            else if (have_flag > 0)
            {
                have_flag--;
                B_click.BackColor = Color.LightYellow;
            }
            flag.Text = $"Flag: {have_flag}";
            if (have_opened + bomb_num == 120 || have_mark == bomb_num)
            {
                openall(0);
                gameover();
                result.Text = $"Success";
            }
        }
        private void openall(int pos)
        {
            have_bomb = 0;
            if (pos < 120 && pos >= 0)
            {
                for (int i = 0; i < bomb_num; i++)
                {
                    if (pos == bomb_pos[i])
                    {
                        ground[pos].Enabled = false;
                        ground[pos].BackColor = Color.PaleVioletRed;
                        openall(pos + 1);
                        return;
                    }
                    else have_bomb += Check_bomb(pos, bomb_pos[i]);
                }
                if (have_bomb > 0)
                {
                    ground[pos].Enabled = false;
                    ground[pos].Text = Convert.ToString(have_bomb);
                }
                else
                {
                    ground[pos].Enabled = false;
                }
                openall(pos + 1);
            }
        }
        private void open(int pos)
        {
            have_bomb = 0;
            if (pos < 120 && pos >= 0 && ground[pos].Enabled)
            {
                for (int i = 0; i < bomb_num; i++)
                {
                    if (pos == bomb_pos[i])
                    {
                        return;
                    }
                    else have_bomb += Check_bomb(pos, bomb_pos[i]);
                }
                if (have_bomb > 0)
                {
                    ground[pos].Enabled = false;
                    ground[pos].Text = Convert.ToString(have_bomb);
                }
                else
                {
                    ground[pos].Enabled = false;
                    if (pos - 15 >= 0) open(pos - 15);
                    if (pos + 15 < 120) open(pos + 15);
                    if (pos % 15 != 0) open(pos - 1);
                    if (pos % 15 != 14) open(pos + 1);
                }
            }
        }

        private int Check_bomb(int pos,int bomb)
        {
            if (pos == bomb - 1 && bomb % 15 != 0) //left{
                return 1;
            else if (pos == bomb + 1 && bomb % 15 != 14) //right
                return 1;
            else if (pos == (bomb - 15)) //up
                return 1;
            else if (pos == bomb + 15) //down
                return 1;
            else if (pos == bomb - 16 && bomb % 15 != 0) //left up
                return 1;
            else if (pos == bomb + 14 && bomb % 15 != 0) //left down
                return 1;
            else if (pos == bomb - 14 && bomb % 15 != 14) //right up
                return 1;
            else if (pos == bomb + 16 && bomb % 15 != 14) //right down
                return 1;
            else return 0;
        }
        private void flag_Click(object sender, EventArgs e)
        {
            if (flag.BackColor == Color.GhostWhite) flag.BackColor = Color.LightYellow;
            else flag.BackColor = Color.GhostWhite;
        }
        private void count_open(object sender,EventArgs e)
        {
            have_opened++;
        }
        /*end game*/
        private void gameover()
        {
            time_trigger.Enabled = false;
            flag.Enabled = false;
            flag.Visible = false;
            
            result.Visible = true;
            restart.Enabled = true;
            restart.Visible = true;

        }
        private void restart_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 120; i++)
            {
                ground[i].Visible = false;
                ground[i].Enabled = false;
            }

            result.Visible = false;

            time_see.Visible = false;
            bomb_see.Visible = false;

            restart.Visible = false;
            restart.Enabled = false;

            Form1_load();
        }
    }
}


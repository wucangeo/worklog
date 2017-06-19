using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.IO;
using Microsoft.Win32;

namespace worklog
{
    public partial class MainForm : Form
    {
        private string logFileName = "worklog.txt";
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_SizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Hide();
                this.notifyIcon1.Visible = true;
            }
        }

        private void notifyIcon1_Click(object sender, EventArgs e)
        {
            
        }

        private void contextMenuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            switch (e.ClickedItem.Name)
            {
                case "tsmi_show":
                    this.Visible = true;
                    this.WindowState = FormWindowState.Normal;
                    break;
                case "tsmi_hide":
                    this.Visible = false;
                    break;
                case "tsmi_exit":
                    this.Close();
                    this.Dispose();
                    break;
                case "tsmi_log":
                    string startPath = System.Environment.CurrentDirectory;
                    string logPath = startPath + "\\"+ logFileName;
                    if (File.Exists(logPath))
                    {
                        System.Diagnostics.Process.Start(logPath);
                    }
                    else
                    {
                        if (MessageBox.Show("工作日志文件未找到,是否重新创建？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
                        {
                            StreamWriter sw;
                            //不存在就新建一个文本文件,并写入一些内容
                            sw = File.CreateText(logFileName);
                            sw.WriteLine("****工作日志****");
                            sw.Close();
                        }
                    }
                    break;
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Visible = false;
            e.Cancel = true;
            return;
        }

        private void notifyIcon1_MouseDown(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Left)
            {
                this.Visible = true;
                this.WindowState = FormWindowState.Normal;
            }
        }

        private void tsmi_auto_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem tsmi = sender as ToolStripMenuItem;
            if (tsmi != null)
            {
                if (tsmi.Checked)
                {
                    //获取程序执行路径..
                    string starupPath = Application.ExecutablePath;
                    //class Micosoft.Win32.RegistryKey. 表示Window注册表中项级节点,此类是注册表装.
                    RegistryKey loca = Registry.LocalMachine;
                    RegistryKey run = loca.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run");

                    try
                    {
                        //SetValue:存储值的名称
                        run.SetValue("worklog", starupPath);
                        MessageBox.Show("已启用开机运行!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        loca.Close();
                    }
                    catch (Exception ee)
                    {
                        MessageBox.Show(ee.Message.ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                }
                else
                {
                    // MessageBox.Show("没有选中");
                    //获取程序执行路径..
                    string starupPath = Application.ExecutablePath;
                    //class Micosoft.Win32.RegistryKey. 表示Window注册表中项级节点,此类是注册表装.
                    RegistryKey loca = Registry.LocalMachine;
                    RegistryKey run = loca.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run");

                    try
                    {
                        //SetValue:存储值的名称
                        run.DeleteValue("worklog");
                        MessageBox.Show("已停止开机运行!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        loca.Close();
                    }
                    catch (Exception ee)
                    {
                        MessageBox.Show(ee.Message.ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}

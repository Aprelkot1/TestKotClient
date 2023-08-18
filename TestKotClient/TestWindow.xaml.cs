using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Shell;

namespace TestKotClient
{
    /// <summary>
    /// Логика взаимодействия для TestWindow.xaml
    /// </summary>
    public partial class TestWindow : Window
    {
        public TestWindow()
        {
            InitializeComponent();

        }
        public void nextQuestion_Click(Object sender, EventArgs e)
        {
            Button tag = sender as Button;
            ((MainWindow)this.Owner).nextQuestion_Click(tag, null);
        }

        public void answer_CheckR(object sender, EventArgs e)//добавляет ответы в список ответов для отправки
        {
            System.Windows.Controls.RadioButton tag = sender as System.Windows.Controls.RadioButton;
            ((MainWindow)this.Owner).answerChekedR_Click(tag.Content.ToString());
        }
        public void answer_UnCheckR(object sender, EventArgs e)//добавляет ответы в список ответов для отправки
        {
            System.Windows.Controls.RadioButton tag = sender as System.Windows.Controls.RadioButton;
            ((MainWindow)this.Owner).answerUnChekedR_Click(tag, null);
        }
        public void answer_CheckChBox(object sender, EventArgs e)//добавляет ответы в список ответов для отправки
        {
            System.Windows.Controls.CheckBox tag = sender as System.Windows.Controls.CheckBox;
            ((MainWindow)this.Owner).answerChekedCh_Click(tag, null);
        }
        public void answer_UnCheckChBox(object sender, EventArgs e)//добавляет ответы в список ответов для отправки
        {
            System.Windows.Controls.CheckBox tag = sender as System.Windows.Controls.CheckBox;
            ((MainWindow)this.Owner).answerUnChekedCh_Click(tag, null);
        }
        public void answerP_isChanged(object sender, EventArgs e)//добавляет ответы в список ответов для отправки
        {
            System.Windows.Controls.ListBox tag = sender as System.Windows.Controls.ListBox;
            StringBuilder sb = new StringBuilder();
            foreach (Answers item in answerListP.Items)
            {
                sb.Append(item.answerText.ToString());
            }
            ((MainWindow)this.Owner).answerP_isChanged(sb.ToString());
        }

    }
}

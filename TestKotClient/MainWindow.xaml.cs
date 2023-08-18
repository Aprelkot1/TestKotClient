using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TestKotClient
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
 
    public partial class MainWindow : Window
    {
        ObservableCollection<Tests> testsList = new ObservableCollection<Tests>(); //коллекция тестов
        ObservableCollection<Answers> answerList = new ObservableCollection<Answers>(); //коллекция ответов 
        TestWindow testWindow = new TestWindow();
        List<string> questionList = new List<string>(); //список вопросов
        int numberOfQuestion;//первый вопрос всегда идет третьим в полученном списке
        List<string> rightAnswers = new List<string>();//список с правильными ответами
        List<string> studentAnswers = new List<string>();
        float studentResult;
        public MainWindow()
        {
            InitializeComponent();
        }
        public void openTest_Click(Object sender, EventArgs e)
        {
            numberOfQuestion = 2;
            questionList.Clear();
            testWindow = new TestWindow();
            Button tag = sender as Button;
            DataStream(tag.Tag.ToString() + "[q]");
        }
        public void answerChekedR_Click(string answer)
        {
            //System.Windows.Controls.RadioButton tag = sender as System.Windows.Controls.RadioButton;
            studentAnswers.Add(answer);
          

        }
        public void answerUnChekedR_Click(Object sender, EventArgs e)
        {
            System.Windows.Controls.RadioButton tag = sender as System.Windows.Controls.RadioButton;
            studentAnswers.Remove(tag.Content.ToString());
        }
        public void answerChekedCh_Click(Object sender, EventArgs e)
        {
            System.Windows.Controls.CheckBox tag = sender as System.Windows.Controls.CheckBox;
            studentAnswers.Add(tag.Content.ToString());
        }
        public void answerUnChekedCh_Click(Object sender, EventArgs e)
        {
            System.Windows.Controls.CheckBox tag = sender as System.Windows.Controls.CheckBox;
            studentAnswers.Remove(tag.Content.ToString());
        }
        public void answerP_isChanged(string answerP)//добавляет ответы в список ответов для отправки
        {
            if (answerP.Trim() != "")
            {
                studentAnswers.Clear();
                studentAnswers.Add(answerP);
            }

           
        }
        public void nextQuestion_Click(Object sender, EventArgs e)
        {
            
            if (studentAnswers.Count > 0)
            {
                Button tag = sender as Button;

                if (rightAnswers.Count == studentAnswers.Count)//если количество ответов студента больше чем количество правильных нет смысла проверять
                {
                    foreach (var sa in studentAnswers)//если ответы студента содержат правильные удаляем их в итоге правильных ответов в списке не должно остаться
                    {

                        if (studentAnswers.Contains(sa))
                        {
                            rightAnswers.Remove(sa);
                        }
                    }
                }
                if (rightAnswers.Count == 0) //если ничего не осталось, то значит все правильно и добавляем правильный ответ в копилку
                {
                    studentResult += 1;
                   
                }
                rightAnswers.Clear();
                if (numberOfQuestion == questionList.Count - 1)
                {
                    string a = (questionList.Count() - 2).ToString();
                    MessageBox.Show("Ваш результат: " + (studentResult / float.Parse(a)*100).ToString() + "%");
                    DataStream(userName.Text + "|" + testWindow.Title + "|" + (studentResult / float.Parse(a) * 100).ToString() + "[end]");
                    testWindow.Close();
                    studentResult = 0;

                }
                else
                {
                    numberOfQuestion += 1;
                    DataStream(questionList[1] + "|" + questionList[numberOfQuestion] + "[qt]");
                   
                }
            }
            else
            {
                MessageBox.Show("Выберите хотя бы один ответ!");
            }
            studentAnswers.Clear();
        }
    }
}

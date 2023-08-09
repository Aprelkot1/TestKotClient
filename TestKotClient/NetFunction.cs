using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static System.Net.Mime.MediaTypeNames;
using System.Threading;
using System.Collections.ObjectModel;

namespace TestKotClient
{
    //[tests] для запроса тестов
    //[q] для вопросов
    //[qt] вопрос с ответами
    //[end] отправка результатов
    public partial class MainWindow : Window
    {
       
      
        public void Connection_Click(object sender, EventArgs e) //завершить тест
        {
            testsList.Clear();
            DataStream(userName.Text + " подключился.[tests]");
        }
        public async void DataStream(string message)
        {
            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(serverIP.Text), 8888);
            using Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            await socket.ConnectAsync(serverIP.Text, 8888);
            using var stream = new NetworkStream(socket);
            try
            {
                // кодируем его в массив байт
                var data = Encoding.UTF8.GetBytes(message);
                // отправляем массив байт на сервер 
                await stream.WriteAsync(data, 0, data.Count());
                while (true)
                {
                    // буфер для получения данных
                    var responseData = new byte[1024];
                    // получаем данные
                    var bytes = await stream.ReadAsync(responseData, 0, responseData.Count());
                    // преобразуем полученные данные в строку
                    string response = Encoding.UTF8.GetString(responseData, 0, bytes);
                    if (response.Contains("[tests]"))
                    {

                        for (int i = 0; i < GetData(response).Count; i++)
                        {
                            
                            Tests testTemp = new Tests
                            {
                                testName = GetData(response)[i]
                            };
                                testsList.Add(testTemp);
                           
                        }
                        testsOut.ItemsSource = testsList;
                    }
                    if (response.Contains("[q]"))
                    {
                        //ответ на запрос вопросов в списке [0] название теста,[1] id теста, далее вопросы 
                        for (int i = 0; i < GetData(response).Count; i++)
                        {
                            questionList.Add(GetData(response)[i].Replace("[q]", ""));
                        }
                        //отправляем запрос на ответы с id теста и первым вопросом,первый вопрос с третьего элемента списка 
                        DataStream(questionList[1] + "|" + questionList[numberOfQuestion] + "[qt]");

                    }
                    if (response.Contains("[qt]"))
                    {
                        answerList.Clear();
                        //[0] - вопрос,[1] - тип вопроса, [2] - количество правильных ответов,[3..] правильные ответы, потом блок всех ответов
                        List<string> questonBlockList = response.Replace("[qt]","").Split('|').ToList();//получаем общий список
                        questonBlockList.RemoveAt(questonBlockList.Count - 1);//удаляем последний элемент там пустота
                        string question = questonBlockList[0];//переменная с вопросом
                        questonBlockList.RemoveAt(0);//удаляем сам вопрос из списка, чтоб не мешался
                        string questionType = questonBlockList[1];// переменная с типом вопроса
                        questonBlockList.RemoveAt(1);//тоже удаляем чтоб не мешалась
                        for (int i = 1; i <= int.Parse(questonBlockList[0]); i++)
                        {
                            rightAnswers.Add(questonBlockList[i]);
                            
                        }
                        int toRemove = int.Parse(questonBlockList[0]) + 1;
                        questonBlockList.RemoveRange(0, toRemove); //удаляем  количество правильных ответов и сами правильные ответы
                        foreach(var answer in questonBlockList)
                        {
                            Answers answerTemp = new Answers
                            {
                                answerText = answer
                            };
                          answerList.Add(answerTemp);
                        }
                        testWindow.Title = questionList[0];
                        if (numberOfQuestion == questionList.Count - 1)
                        {
                          
                            testWindow.answerButton.Content = "Завершить";
                        }
                        else
                        {
                            testWindow.answerButton.Content = "Далее";
                        }
                        testWindow.Owner = this;
                        //текст вопроса в тестовом окне
                        testWindow.questionBlock.Text = question;
                        if (questionType.Contains("Несколько вариантов"))
                        {
                            testWindow.answerListCh.Visibility = Visibility.Visible;
                            testWindow.answerListR.Visibility = Visibility.Hidden;
                            testWindow.answerListCh.ItemsSource = answerList;
                        }
                        if (questionType.Contains("Один вариант"))
                        {
                            testWindow.answerListCh.Visibility = Visibility.Hidden; 
                            testWindow.answerListR.Visibility = Visibility.Visible;
                            testWindow.answerListR.ItemsSource = answerList;
                        }
                        testWindow.Show();
                        
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
       
        public List<string> GetData(string response)
        {
            List<string> dataResult = new List<string>();
            dataResult = response.Split('|').ToList();
            dataResult.RemoveAt(dataResult.Count - 1);
            return dataResult;
        }
    }
    public class Tests
    {
        public string testName { get; set;}
    }
    public class Answers
    {
        public string answerText{ get; set; }
    }
}
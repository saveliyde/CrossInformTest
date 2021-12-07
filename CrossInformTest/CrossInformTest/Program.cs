using System.Diagnostics;

namespace CrossInformApp
{
    class TripletArray
    {
        private int threadCount= 3;                                                     //Количество потоков, хотел сначала исользовать для создания массива потоков,
                                                                                        //чтобы менять можно было, но с многопоточностью намучился и вручную сам таски прописал

        private List<string> ThreeLettersArray = new List<string>();                    //Идея была в том, что я передаю текст, многопоточно обрабатываю и достаю триплеты,
        private Dictionary<String, int> Result = new Dictionary<String, int>();         //и уже их поочередно переношу в словарь

        private string[] s;
        private string s1;                                                              //Выглядит некрасиво
        private string s2;
        
        public TripletArray(string[] s)
        {
            this.s = s;
        }
        
        public void Run(object i)                                                        //Сама обработка. Передаю массив слов в тексте, распределяю их между потоками и раскладываю 
        {                                                                                //на триплеты, которые передаю в список.
            for (int x = (int)i; x < s.Length - threadCount; x = x + threadCount)
            {
                s1 = s[x];
                {
                    for (int j = 0; j < s1.Length - 2; j++)
                    {
                        if (char.IsLetter(s1[j]) & char.IsLetter(s1[j + 1]) & char.IsLetter(s1[j + 2])) //Здесь постоянно ошибки вылетают, но я пока не очень понял как они вообще проходят
                        {
                            s2 = s1.Substring(j, 3);
                            ThreeLettersArray.Add(s2);
                        }
                    }
                }
            }
        }

        public void Start()                                                                //Потоки
        {
            Task task1 = Task.Factory.StartNew(() => Run(1));
            Task task2 = Task.Factory.StartNew(() => Run(2));
            Task task3 = Task.Factory.StartNew(() => Run(3));

            Task.WaitAll(task1, task2, task3);
            Read();
        } 
        
        public void Read()                                                                   //Вывод того, что наобрабатывал
        {
            foreach (string triplet in ThreeLettersArray)
            {
                if (Result.ContainsKey(triplet))
                    Result[triplet]++;
                else
                    Result.Add(triplet, 1);

            }
            foreach (var pair in Result.OrderByDescending(pair => pair.Value).Take(10))
                Console.WriteLine(pair.Key + " " + pair.Value);
        }
}
    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch stopWatch = new Stopwatch();                                      //Таймер
            stopWatch.Start();

            //string adress1 = Console.ReadLine();
            //string s = System.IO.File.ReadAllText(adress1, System.Text.Encoding.GetEncoding(1251));
            string s = "Ребята, не стоит вскрывать эту тему. Вы молодые, шутливые, вам всё легко. Это не то. Это не Чикатило и даже не архивы спецслужб. " +        //Тестовый текст
                "Сюда лучше не лезть. Серьезно, любой из вас будет жалеть. Лучше закройте тему и забудьте что тут писалось. Я вполне понимаю, что данным " +
                "сообщением вызову дополнительный интерес, но хочу сразу предостеречь пытливых - стоп. Остальные просто не найдут.";
            string[] s1 = s.Split();

            TripletArray arr = new TripletArray(s1);
            arr.Start();

            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;                                      //Таймер
            string elapsedTime = String.Format("{0:00} часов, {1:00} минут, {2:00} секунд, {3:000} миллисекунд",
                        ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds);
            Console.WriteLine("Время выполнения программы: " + elapsedTime);
        }
        
    }   
    
}
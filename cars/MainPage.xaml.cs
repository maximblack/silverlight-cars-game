using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Threading;


using System.Windows.Media.Imaging;

namespace cars
{
	public partial class MainPage : UserControl
	{
		//declar 4 timere pentru 4 obiecte care vor aparea pe harta.In cazul meu
		//masina de sus,jos,dreapta,stanga
		public System.Windows.Threading.DispatcherTimer bottom_car_timer;
		public System.Windows.Threading.DispatcherTimer top_car_timer;
		public System.Windows.Threading.DispatcherTimer left_car_timer;
		public System.Windows.Threading.DispatcherTimer right_car_timer;
		public System.Windows.Threading.DispatcherTimer colission_timer;
		
		public int timer_current_milliseconds = 4000; //8000//reprezinta timpul pentru ca o masina sa parcurga pathul propus
		public int timer_max_milliseconds = 2000; //4000
		public int timer_min_milliseconds = 250; //500
		public int timer_milliseconds_step = 50;
		
		public int current_score_value = 0;//scorul de acumulat
		public int max_score_value = 10;//scor bydefault egal cu 10
		
		public double initialSpeed = 1;//viteza initiala
		public double actionSpeed = 1.5;//viteza dupa apasarea butonului de accelerare
		public double speedStep = 0.05;//pasul de accelerare 
		public int speedMilliseconds = 10;//timpul de accelerare
		
		
		public int default_duration = 4;//numarul de keyFrame-uri in care masina parcurge calea
		public int check_collision_timer_duration= 100;
		
		public bool isGameOver = true;
		
		/// <summary>
		/// Acest rand creaza o instanta a obiectului Random pe care o sa-l 
		/// folosim la imaginile de masini pentru a crea o alegere random a cestora pe pathuri-le animatiilor
		/// </summary>
		public Random random = new Random();
		/// <summary>
		/// Functia de initiaizare component care este strict necesara pentru 
		/// rularea aplicatiei
		/// </summary>
		public MainPage()
		{
			InitializeComponent();
		}
		/// <summary>
		/// Acesta metoda este pentru crearea unui timer care va fi folosit la fiecare Imagine obiect a fiecarei masini
		/// se creaza un timer "timer" si se stabileste un interval random de pornire.
		///  </summary>
		/// <param name="callback"></param>
		/// <param name="start"></param>
		/// <returns></returns>
		public System.Windows.Threading.DispatcherTimer attachTimer(EventHandler callback, bool start) {
			System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer();
			timer.Interval = new TimeSpan(0, 0, 0, 0, getRandomInterval());
			timer.Tick += callback;
			if(start)
				timer.Start();
			return timer;
		}
		/// <summary>
		/// Intervalul de pornire a timer-ului este setat in acesta functie si apelat in attachTimer
		/// </summary>
		/// <returns></returns>
		public int getRandomInterval() {
			return random.Next(timer_min_milliseconds, timer_current_milliseconds);
		}
		
		public void decrementTimerMilliseconds() {
			if(timer_current_milliseconds > timer_max_milliseconds)
				timer_current_milliseconds -= timer_milliseconds_step;	
		}
		/// <summamary>
		/// Functie pentru miscarea masinii pe un drum.
		/// Acest path se numeste storyboard.Pentru ca masina sa se miste continuu pe acest drum 
		/// am creat aceasta functie.
		///
		/// </summary>
		/// <param name="state"></param>
		/// <param name="sender"></param>
		public void bottom_car_timer_callback(object state, EventArgs sender) {
			
			bottom_car_timer.Stop();//Stopez timer-ul pentru a nu-l porni de 2 ori
			
			decrementTimerMilliseconds();//decrementez timpul acumulat 
			
			bottom_car_timer.Interval = new TimeSpan(0, 0, 0, 0, getRandomInterval());//setez un interval de reaparitie pe harta
			
			randomCar(bottom_car);//iau o imagine random pentru a evita aparitia aceleiasi imagini pe harta
			
			bottom_car_storyboard.SpeedRatio = initialSpeed;//selectez o viteza initiala a masinii
			
			bottom_car_storyboard.Begin();//incep din nou parcurgerea pe harta.
			
		}
		/// <summary>
		/// Top_car_timer_callback
		/// </summary>
		/// <param name="state"></param>
		/// <param name="sender"></param>
		
		public void top_car_timer_callback(object state, EventArgs sender) {
			
			top_car_timer.Stop();
			
			decrementTimerMilliseconds();
			
			top_car_timer.Interval = new TimeSpan(0, 0, 0, 0, getRandomInterval());
			
			randomCar(top_car);
			
			top_car_storyboard.SpeedRatio = initialSpeed;
			
			top_car_storyboard.Begin();
			
		}
		
		public void left_car_timer_callback(object state, EventArgs sender) {
			
			left_car_timer.Stop();
			
			decrementTimerMilliseconds();
			
			left_car_timer.Interval = new TimeSpan(0, 0, 0, 0, getRandomInterval());
			
			randomCar(left_car);
			
			left_car_storyboard.SpeedRatio = initialSpeed;
			
			left_car_storyboard.Begin();
			
		}
		
		public void right_car_timer_callback(object state, EventArgs sender) {
			
			right_car_timer.Stop();
			
			decrementTimerMilliseconds();
			
			right_car_timer.Interval = new TimeSpan(0, 0, 0, 0, getRandomInterval());
			
			randomCar(right_car);
			
			right_car_storyboard.SpeedRatio = initialSpeed;
			
			right_car_storyboard.Begin();
			
		}
		/// <summary>
		/// Aceasta functie trateaza evenimentele de apasare a butoanelor
		/// sus,jos,dreapta,stanga pentru aceleratia masinii in dependenta de
		/// drumul ei(spe dreapta,stanga ,sus sau jos)
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void UserControl_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
		{
			switch(e.Key) {
				case System.Windows.Input.Key.Up:
					changeSpeed(bottom_car_storyboard);
					break;
				case System.Windows.Input.Key.Down:
					changeSpeed(top_car_storyboard);
					break;
				case System.Windows.Input.Key.Left:
					changeSpeed(right_car_storyboard);
					break;
				case System.Windows.Input.Key.Right:
					changeSpeed(left_car_storyboard);
					break;
			}
		}
		/// <summary>
		/// Change speed raspunde pentru schimbarea vitezei unei masini
		/// </summary>
		/// <param name="storyboard"></param>
		public void changeSpeed(Storyboard storyboard) {
			System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer();
			timer.Interval = new TimeSpan(0, 0, 0, 0, speedMilliseconds);//se da un interval 
			
			EventHandler handler = null;//se seteaza evenimentul de apasare
		
			handler = (s, e) =>
			{
				changeSpeedCallback(storyboard, timer);//se schimba viteza 
			};
		
			timer.Tick += handler;
			timer.Start();
		}
		/// <summary>
		/// schimba viteza unei masini in decursul animatiei
		/// </summary>
		/// <param name="storyboard"></param>
		/// <param name="timer"></param>
		public void changeSpeedCallback(Storyboard storyboard, System.Windows.Threading.DispatcherTimer timer) {
			if(storyboard.SpeedRatio < actionSpeed)
				storyboard.SpeedRatio += speedStep;
			else
				timer.Stop();
				
		}
		/// <summary>
		/// Se seteaza numarul de masini(imaginile acesteia)
		/// </summary>
		/// <param name="image"></param>
		public void randomCar(Image image) {
			int carsNumber = 4;
			Random random = new Random();
			image.Source = new BitmapImage(new Uri("car" + random.Next(1, carsNumber + 1) + ".png", UriKind.Relative));	//sursa imaginii		
		}
		
		//Functie pentru atasare a unui raspuns la un eveniment
		public void attachCallback(Storyboard storyboard, Action callback)
		{
			EventHandler handler = null;
		
			handler = (s, e) =>
			{
				callback();
				storyboard.Completed -= handler;
			};
		
			storyboard.Completed += handler;
			
		}
		/// <summary>
		/// Aceasta functie se apeleaza atunci cand o masina termina drumul predefint
		/// si pentru a incepe un ciclu nou pe acest drum avem nevoie de aceasta functie.
		/// </summary>
		public void bottom_car_storyboard_completed()
		{
			
			bottom_car_storyboard.Stop();//terminarea drumului
			
			attachCallback(bottom_car_storyboard, bottom_car_storyboard_completed);//atasarea evenimentului care stie ca 
			//animatia a luat sfarsit(a trecut timpul pentru o deplasare)
			
			bottom_car_timer.Start();//se reinitializeaza timer-ul
			
			IncrementCurrentScore();//se incrementeaza numarul de masini cu anmatii complete
		}
		public void top_car_storyboard_completed()
		{
			top_car_storyboard.Stop();
			
			attachCallback(top_car_storyboard, top_car_storyboard_completed);
			
			top_car_timer.Start();
			
			IncrementCurrentScore();
		}
		public void left_car_storyboard_completed()
		{
			left_car_storyboard.Stop();
			
			attachCallback(left_car_storyboard, left_car_storyboard_completed);
			
			left_car_timer.Start();
			
			IncrementCurrentScore();
		}
		public void right_car_storyboard_completed()
		{
			right_car_storyboard.Stop();
			
			attachCallback(right_car_storyboard, right_car_storyboard_completed);
			
			right_car_timer.Start();
			
			IncrementCurrentScore();
		}
		//Functia care raspunde pentru setarile si pornirea jocului
		public void InitializeGame() {
		//pentru fiecare timer creat eu atasez o functie attachTimer si ii dau ca parametru un event si anume de de pornire 
			//a unei noi animatii
			bottom_car_timer = attachTimer(new EventHandler(bottom_car_timer_callback), true);
			top_car_timer = attachTimer(new EventHandler(top_car_timer_callback), true);
			left_car_timer = attachTimer(new EventHandler(left_car_timer_callback), true);
			right_car_timer = attachTimer(new EventHandler(right_car_timer_callback),true);
			//se ataseaza un eveniment care are ca parametru animatia si terminarea cu reiniializare a acesteia
			attachCallback(bottom_car_storyboard, bottom_car_storyboard_completed);
			attachCallback(top_car_storyboard, top_car_storyboard_completed);
			attachCallback(left_car_storyboard, left_car_storyboard_completed);
			attachCallback(right_car_storyboard, right_car_storyboard_completed);
			//butonul de start al jocului care este visibil doar la pornire si pana la apasare
			button_start_game.Visibility = Visibility.Collapsed;
			//se seteaza scorul curent si scorul maxim setat by defaul 10
			SetCurrentScore(current_score_value);
			SetMaxScore(max_score_value);
			//se analizeaza durata fiecarei animatii si se initializeaza timpul pentru ca o masina sa termine drumul de parcurs
			bottom_car_storyboard.Duration = new Duration(new TimeSpan(0, 0, default_duration));
			top_car_storyboard.Duration = new Duration(new TimeSpan(0, 0, default_duration));
			left_car_storyboard.Duration = new Duration(new TimeSpan(0, 0, default_duration));
			right_car_storyboard.Duration = new Duration(new TimeSpan(0, 0, default_duration));
			//se initializeaza un timer care o sa verifice la fiecare 50 milisecunde situatia masinilor pe harta
			//are scop de verificare a coleziunii
			colission_timer = new System.Windows.Threading.DispatcherTimer();
			colission_timer.Interval = new TimeSpan(0,0,0,0,check_collision_timer_duration);
			colission_timer.Tick += new EventHandler(CheckCarsCollission);
			colission_timer.Start();
			
			isGameOver = false;
			
		}
		
		public void GameOver() {
			
			if(isGameOver)
				return;
			
			button_start_game.Visibility = Visibility.Visible;
			
			bottom_car_storyboard.Stop();
			top_car_storyboard.Stop();
			left_car_storyboard.Stop();
			right_car_storyboard.Stop();
			
			bottom_car_timer.Stop();
			top_car_timer.Stop();
			left_car_timer.Stop();
			right_car_timer.Stop();
			colission_timer.Stop();
			
			
			MessageBox.Show("Jocul sa terminat. Scorul tau: " + current_score_value);
			
			
			isGameOver = true;
			
			current_score_value = 0;
			
		}
		
		//metoda de pornire a jocului in care apelez functia InitializeGame()
		private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			
			this.InitializeGame();
			
		}
		/// <summary>
		/// Incrementarea scorului curent
		/// </summary>
		public void IncrementCurrentScore() {
			current_score_value++;
			SetCurrentScore(current_score_value);
			if(current_score_value > max_score_value) {
				max_score_value = current_score_value;
				SetMaxScore(max_score_value);	
			}
		}
		
		public void SetCurrentScore(int score) {
			score_current.Text = score.ToString();	
		}
		
		public void SetMaxScore(int score) {
			score_max.Text = score.ToString();
		}
		/// <summary>
		/// Functie care ia coordonatele imaginii pe harta(masinii)
		/// </summary>
		/// <param name="image"></param>
		/// <returns></returns>
		public double[] GetImageOptions(Image image)
		{
		
			double[] options = new double[4];
			
			double leftProperty = (double)image.GetValue(Canvas.LeftProperty);
			double topProperty = (double)image.GetValue(Canvas.TopProperty);
			double offsetX = (double)image.RenderTransform.GetValue(CompositeTransform.TranslateXProperty);
			double offsetY = (double)image.RenderTransform.GetValue(CompositeTransform.TranslateYProperty);
			
			options[0] = leftProperty + offsetX;
			options[1] = topProperty + offsetY;
			
			double rotation = (double)image.RenderTransform.GetValue(CompositeTransform.RotationProperty);
			
			if(Math.Abs(rotation) == 90) {
				options[2] = image.Height;
				options[3] = image.Width;
			} else {
				options[2] = image.Width;
				options[3] = image.Height;	
			}
			
			return options;
		}
		/// <summary>
		/// Functie care returneaza coordonatele colturilor imaginii
		/// </summary>
		/// <param name="image"></param>
		/// <returns></returns>
		public double[][] GetImageCorners(Image image)
		{
			
			double[] options = GetImageOptions(image);
			double[][] corners = new double[4][];
			corners[0] = new double[2] {options[0],options[1]};
			corners[1] = new double[2] {options[0]+options[2],options[1]};
			corners[2] = new double[2] {options[0]+options[2],options[1]+options[3]};
			corners[3] = new double[2] {options[0],options[1]+options[3]};
			
			//calculeaza toate xy ale unghiurilor
			return corners;
			
			
		}
		/// <summary>
		/// Functie care verifica daca a fost sau nu vreo coleziune intre masini
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		public void CheckCarsCollission(object sender, EventArgs args)
		{
			
			colission_timer.Stop();
			try{
			
			
			Image[] cars = {bottom_car, left_car, top_car,  right_car};
			int cars_count = cars.Count();
			
			for(int carIterator = 0 ; carIterator < cars_count ; carIterator++)
			{
				
				double[] carOptions = GetImageOptions(cars[carIterator]);
			
				
				for(int carIteratorChild = 0; carIteratorChild < cars_count ; carIteratorChild++)
				{
					
					if(carIterator != carIteratorChild)
					{
						
						
						double[][] corners  = GetImageCorners(cars[carIteratorChild]);
						
						for(int cornerIterator = 0;cornerIterator < 4; cornerIterator++)
						{
							
							
							
							if(carOptions[0] <= corners[cornerIterator][0] && corners[cornerIterator][0] <= (carOptions[0] + carOptions[2])
								&& carOptions[1] <= corners[cornerIterator][1] && corners[cornerIterator][1] <= (carOptions[1] + carOptions[3]))
							{
								
								GameOver();
								
							}
							
							
							
						}
						
						
						
						
						
						
					}
					

				}
				//log("--");
				
			}
		
			} catch(Exception e) {
						log(e.ToString());	
						}
			colission_timer.Start();
			
		}
		public void log(string data) {
			logtextblock.Text += data + "\n";
		}
		
		
		
		
	}
	
	
		
	
}

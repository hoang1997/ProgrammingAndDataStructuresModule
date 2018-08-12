using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;

namespace programming_assessment_2.cs
{
	//class to create die and return a random number
	class DieClass
	{
		private int topNumber;

		private Random randomNumber;

		//create conctructor
		public DieClass(Random numberProducer)
		{
			randomNumber = numberProducer;
		}

		//roll the dice and assign topNumber a random number 
		public void roll()
		{
			topNumber = randomNumber.Next(1, 7);
		}

		//return the top number
		public int returnTopNumber()
		{
			return topNumber;
		}
	}

	//class to create get players name, and get players score 
	class PlayerClass
	{
		//method to get players name
		public string getPlayerName()
		{
			//create a empty string
			string playerName = "";

			//while loop to get users input, and it will keep looping till they enter a name. Make sure they dont accidentally press enter
			while (playerName.Length < 1)
			{
				Console.WriteLine("Please enter player name.");
				playerName = Console.ReadLine();

				if (playerName.Length < 1)
				{
					Console.WriteLine("\nPlease enter a valid name.");
					Console.Clear();

				}
			}

			return playerName;
		}

		//create player score
		private int playerScore;
		public int currentScore { get { return playerScore; } }

		//set score as the current score plus the new value
		public void scoreSet(int NewScore)
		{
			playerScore = currentScore + NewScore;
		}

		//get the score and return the current value
		public int getScore()
		{
			return playerScore;
		}
	}

	class GameClass
	{
		//new instance of the stats class
		StatisticsClass statsClass = new StatisticsClass();
		//array to keep track of frequency of each number.
		private int[] numberAppearances = new int[7];

		//list to store the the actual roll value
		private List<int> numbersList = new List<int>();

		public int newGame()
		{
			//create playerscore and set to 0
			int playerScore = 0;
			int playerInput;
			try
			{
				Console.WriteLine("Would you like to: 1. Throw each dice once at a time 2. Throw all at once");
				playerInput = Convert.ToInt32(Console.ReadLine());
				switch (playerInput)
				{
					case 1:
						//create dice
						Random randomiser = new Random();
						DieClass[] playersDice = new DieClass[5];

						//for loop which rolls dice 5 times
						for (int i = 0; i < 5; i++)
						{
							playersDice[i] = new DieClass(randomiser);
							playersDice[i].roll();

							int numberRolled = playersDice[i].returnTopNumber();
							numbersList.Add(numberRolled);
							numberAppearances[numberRolled]++;

							Console.WriteLine("Players Rolled: {0}", numberRolled);

							Console.ReadLine();
						}
						if (numberAppearances.Contains(2))
							{ Console.WriteLine("Rolled a 2 of a kind!"); }
							else if (numberAppearances.Contains(3))
							{ Console.WriteLine("Rolled a 3 of a kind!"); }
							else if (numberAppearances.Contains(4))
							{ Console.WriteLine("Rolled a 4 of a kind!"); }
							else if (numberAppearances.Contains(5))
							{ Console.WriteLine("Rolled a 5 of a kind!"); }
						//sets player score
						playerScore = pointsMethod(numberAppearances, playerScore);

						//prints dice roll history
						statsClass.diceRollHistory(numbersList);

						//prints out stats
						Console.WriteLine("\n\nAll Dice In Game Statistics:\nAverage {0}", statsClass.averageNumberRolled(numbersList));
						Console.WriteLine("Sum: {0}\n", statsClass.sumOfDice(numbersList));

						//clears the frequency array so it can be used again with it being empty. 
						Array.Clear(numberAppearances, 0, numberAppearances.Length);
						break;

					case 2:
						//calling throwAllDice method
						throwAllDice();

						//getting players score and multiply by two because they rolled all at once
						playerScore = (pointsMethod(numberAppearances, playerScore)) * 2;

						statsClass.diceRollHistory(numbersList);
						Console.WriteLine("\n\nAll Dice In Game Statistics\nAverage: {0}", statsClass.averageNumberRolled(numbersList));
						Console.WriteLine("Sum: {0}\n", statsClass.sumOfDice(numbersList));

						Array.Clear(numberAppearances, 0, numberAppearances.Length);
						break;
				}
			}
			//catches format errors when user inputs a key
			catch (FormatException)
			{ Console.WriteLine("Please Enter A valid key"); }

			return playerScore;
		}

		//method for AI
		public int AIgame()
		{
			//setting AI score to 0 
			int AIscore = 0;
			//automatically throws all dice
			throwAllDice();

			//setting AI score
			AIscore = (pointsMethod(numberAppearances, AIscore)) * 2;

			Console.WriteLine("All Dice In Game Statistics\nAverage: {0}", statsClass.averageNumberRolled(numbersList));
			Console.WriteLine("Sum: {0}\n", statsClass.sumOfDice(numbersList));

			Array.Clear(numberAppearances, 0, numberAppearances.Length);

			return AIscore;

		}

		//method to calculate points
		public int pointsMethod(int[] numberAppearances, int playerScore)
		{
			//loop through numberAppearances for frequency of the rolled dice in that round
			for (int i = 0; i < numberAppearances.Length; i++)
			{
				//if there is a 3 of a kind
				if (numberAppearances[i] == 3)
				{
					playerScore = playerScore + 3;
					break;
				}
				//if there is a 4 of a kind
				else if (numberAppearances[i] == 4)
				{
					playerScore = playerScore + 6;
					break;
				}
				//if there is a 5 of a kind
				else if (numberAppearances[i] == 5)
				{
					playerScore = playerScore + 12;
					break;
				}
				//if there is a double, let the player rethrow other 3 dice. 
				else if (numberAppearances[i] == 2)
				{
					//create 3 dice
					Random randomiser = new Random();
					int[] newNumApp = new int[7];
					int doubleNum = Array.IndexOf(numberAppearances, 2);
					int[] newDice = new int[3];
					Console.WriteLine("{0} was thrown twice, Press enter to rethrow other 3 dice!",doubleNum);
					Console.ReadLine();

					//increment the double number by 2 in the new frequency array
					for (int j = 0; j < 2; j++)
					{
						newNumApp[doubleNum]++;
					}

					//roll dice 3 times
					for (int j = 0; j < 3; j++)
					{
						newDice[j] = randomiser.Next(1, 7);

						newNumApp[newDice[j]]++;

						Console.WriteLine("Roll {0} = {1}", j + 1, newDice[j]);
						Console.ReadLine();
						numbersList.Add(newDice[j]);
					}

					//reset the score
					if (newNumApp.Contains(3))
					{ playerScore += 3; }
					else if (newNumApp.Contains(4))
					{ playerScore += 6; }
					else if (newNumApp.Contains(5))
					{ playerScore += 12; }
					break;
				}
			}

			return playerScore;
		}

		public void throwAllDice()
		{
			//create a 5 dice
			Random randomiser = new Random();
			DieClass[] playersDice = new DieClass[5];

			//roll dice 5 times
			for (int i = 0; i < 5; i++)
			{
				playersDice[i] = new DieClass(randomiser);
				playersDice[i].roll();
				int numberRolled = playersDice[i].returnTopNumber();

				numbersList.Add(numberRolled);

				numberAppearances[numberRolled]++;

				Console.WriteLine("Players Rolled: {0}", numberRolled);
			}

			//print what player got
			if (numberAppearances.Contains(2))
			{ Console.WriteLine("Rolled a 2 of a kind!"); }
			else if (numberAppearances.Contains(3))
			{ Console.WriteLine("Rolled a 3 of a kind!"); }
			else if (numberAppearances.Contains(4))
			{ Console.WriteLine("Rolled a 4 of a kind!"); }
			else if (numberAppearances.Contains(5))
			{ Console.WriteLine("Rolled a 5 of a kind!"); }
		}
	}

	//calculates some stats of game
	class StatisticsClass
	{
		//calculates average number of whole game
		public int averageNumberRolled(List<int> numberList)
		{
			//get the list which contains all the numbers of the game so far and find out the sum.
			int sum = numberList.Sum();


			int average = sum / numberList.Count;

			return average;
		}

		//calculates sum of dice in the whole game
		public int sumOfDice(List<int> numberList)
		{
			int sum = numberList.Sum();

			return sum;
		}

		//displays dice history of game
		public void diceRollHistory(List<int> numberList)
		{
			//turn the numbers list into an array
			int[] rollArray = numberList.ToArray();

			Console.WriteLine("Dice Roll History: ");
			//loop through array and print number
			for (int i = 0; i < rollArray.Length; i++)
			{
				Console.Write(rollArray[i] + " ");

			}
		}
	}

	class MainClass
	{
		public static void Main(string[] args)
		{
			//new instance of game
			GameClass newGameRound = new GameClass();

			//two instances of the player as there are 2 player
			PlayerClass playerOne = new PlayerClass();
			PlayerClass playerTwo = new PlayerClass();
			//new instance of the AI player
			PlayerClass AI = new PlayerClass();

			//create a list of all of player and ai scores
			List<int> playerOneScores = new List<int>();
			List<int> playerTwoScores = new List<int>();
			List<int> AIscores = new List<int>();

			//set players score
			int playerOneScore = 0;
			int playerTwoScore = 0;
			int AIscore = 0;

			while (true)
			{
				while (true)
				{
					Console.WriteLine("Would you like to play a friend or the AI? 1. Friend 2. AI");

				try
				{
					int userChoice = Convert.ToInt32(Console.ReadLine());

					if (userChoice == 1)
					{	//create empty string for the names
						string playerOneName = "";
						string playerTwoName = "";
						//while the first players name = to the second players name and if it doesnt it breaks out of the loop
						while (playerOneName == playerTwoName)
						{
							//get method of getting players name		
							playerOneName = playerOne.getPlayerName();
							playerTwoName = playerTwo.getPlayerName();

							if (playerOneName == playerTwoName)
							{
								Console.WriteLine("Please enter different names, press enter to continue");
								Console.ReadLine();
								Console.Clear();
							}
						}
						Console.Clear();
						//while both players havent won
						while (playerOne.getScore() <= 25 || playerTwo.getScore() <= 25)
						{
							Console.WriteLine(playerOneName + "'s Turn\n");
							//get players score by playing a round of the game
							playerOneScore = newGameRound.newGame();
							//set players score
							playerOne.scoreSet(playerOneScore);
							//add players score to the list
							playerOneScores.Add(playerOneScore);
							Console.WriteLine("{0}'s current score is {1}\n", playerOneName, playerOneScore);
							Console.WriteLine("{0}'s Total Score: {1}", playerOneName, playerOneScores.Sum());
							Console.ReadLine();
							Console.Clear();

								// if player has 25 points
								if (playerOneScores.Sum() >= 25)
								{
									Console.WriteLine("{0} has Won!", playerOneName);
									break;
								}

							// if player one hasnt won, then its players 2 turn
							Console.WriteLine(playerTwoName + "'s Turn\n");
							playerTwoScore = newGameRound.newGame();
							playerTwo.scoreSet(playerTwoScore);
							playerTwoScores.Add(playerTwoScore);
							Console.WriteLine("{0}'s current score is {1}\n", playerTwoName, playerTwoScore);
							Console.WriteLine("{0}'s Total Score: {1}", playerTwoName, playerTwoScores.Sum());
							Console.ReadLine();
							Console.Clear();
							//check to see if player two has won
							if (playerTwoScores.Sum() >= 25)
							{
								Console.WriteLine("{0} has Won!", playerTwoName);
								break;
							}
						}
						break;
					}
					else if (userChoice == 2)
					{
						//set players name =		
						string playerOneName = playerOne.getPlayerName();
						Console.Clear();
						//while player and ai has not won
						while (playerOne.getScore() <= 25 || AI.getScore() <= 25)
						{
							//players turn		
							Console.WriteLine(playerOneName + "'s Turn\n");
							playerOneScore = newGameRound.newGame();
							playerOne.scoreSet(playerOneScore);
							playerOneScores.Add(playerOneScore);
							Console.WriteLine("{0}'s current score is {1}\n", playerOneName, playerOneScore);
							Console.WriteLine("{0}'s Total Score: {1}", playerOneName, playerOneScores.Sum());
							Console.ReadLine();
							Console.Clear();

							//check to see if player has won
							if (playerOneScores.Sum() >= 25)
							{
								Console.WriteLine("{0} has Won!", playerOneName);
								break;
							}
							//if player has not won, then AI's turn 
							Console.WriteLine("AI's Turn\n");
							AIscore = newGameRound.AIgame();
							AI.scoreSet(AIscore);
							AIscores.Add(AIscore);
							Console.WriteLine("AI's current Score is {0}\n", AIscore);
							Console.WriteLine("AI's Total Score: {0}", AIscores.Sum());
							Console.ReadLine();
							Console.Clear();
							
								//check to see if ai has won
							if (AIscores.Sum() >= 25)
							{
								Console.WriteLine("You Lost! AI has WON!");
								break;
							}
						}
						break;
					}
				}
				//catch a format exception
				catch (FormatException)
				{
					Console.WriteLine("Please enter a valid key 1 or 2\n");
				}
			}
		
			Console.WriteLine("Would you like to play again? 1. Yes 2. No");
			//get users input to see if they want to play again. or it breaks out of loop and closes program
			try
			{

			int userInput3 = Convert.ToInt32(Console.ReadLine());

				if (userInput3 == 2)
				{
					break; //break out of main while loop.
				}
			}
			catch (FormatException)
			{
				Console.WriteLine("Please enter a valid key\nWould you like to play again? 1.Yes 2.No");
			}
		}
	}
}
}

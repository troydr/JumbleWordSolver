using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;


namespace WordSolver
{
    public partial class Form1 : Form
    {
        Dictionary<string, List<string>> wordDict = new Dictionary<string, List<string>>();


        Dictionary<string, List<string>> wordDict2 = new Dictionary<string, List<string>>();
        Dictionary<string, List<string>> wordDict3 = new Dictionary<string, List<string>>();
        Dictionary<string, List<string>> wordDict4 = new Dictionary<string, List<string>>();
        Dictionary<string, List<string>> wordDict5 = new Dictionary<string, List<string>>();
        Dictionary<string, List<string>> wordDict6 = new Dictionary<string, List<string>>();

        //List<List<string>> solutions = new List<List<string>>();

        List<string> solutionList = new List<string>();

        long solutionsFound = 0;
        long lettersAdded = 0;
        long wordsFound = 0;
        long wordsRejected = 0;
        long recursiveCalls = 0;


        Dictionary<string, bool> sub2 = null;
        Dictionary<string, bool> sub3 = null;
        Dictionary<string, bool> sub4 = null;
        Dictionary<string, bool> sub5 = null;
        Dictionary<string, bool> sub6 = null;

        List<Dictionary<string, bool>> subsetDictionaryList = new List<Dictionary<string, bool>>();



        public Form1()
        {
            InitializeComponent();
            wordDict = LoadDictionary("2to8");
        }


        private void ClearTrackingNumbers()
        {
            solutionsFound = 0;
            lettersAdded = 0;
            wordsFound = 0;
            wordsRejected = 0;
            recursiveCalls = 0;

        }
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {

                string appPath = Application.StartupPath;
                string dataPath = appPath.Substring(0, appPath.IndexOf("WordSolver\\bin"));
                dataPath += "Data";
                string[] lines = File.ReadAllLines(@"C:\source\wordsEn.txt");

                System.IO.StreamWriter file = new System.IO.StreamWriter(@"C:\source\words2to8.txt");
                foreach (string line in lines)
                {
                    if (line.Length >= 2 && line.Length <= 8 && HasAVowel(line))
                    {
                        file.WriteLine(line);
                    }
                }

                file.Close();
            }
            catch(Exception ex)
            {
                int foo;
                foo = 1;
            }
        }

        private void SaveOneWordSizeDictionary(int wordSize)
        {
            System.IO.StreamWriter file = new System.IO.StreamWriter(@"C:\source\words" + wordSize + ".txt");
            try
            {
                Dictionary<string, List<string>> wordDict = new Dictionary<string, List<string>>();

                string[] lines = File.ReadAllLines(@"C:\source\words2to8.txt");                

                foreach (string line in lines)
                {
                    string word = line.Trim().ToLower();

                    if (word.Length == wordSize)
                    {

                        string letters = Alphabetize(word);

                        if (wordDict.ContainsKey(letters))
                        {
                            wordDict[letters].Add(word);
                        }
                        else
                        {
                            List<string> newWordList = new List<string>();
                            newWordList.Add(word);
                            wordDict.Add(letters, newWordList);

                            file.WriteLine(letters);
                        }
                    }
                }
             
                file.Close();

            }
            catch (Exception ex)
            {
                file.Close();
            }
        }



        private Dictionary<string, List<string>> LoadDictionary(string name)
        {

            try
            {
                Dictionary<string, List<string>> wordDict = new Dictionary<string, List<string>>();

                string[] lines = File.ReadAllLines(@"C:\source\words" + name + ".txt");//2to8
                foreach (string line in lines)
                {
                    string word = line.Trim().ToLower();
                    string letters = Alphabetize(word);

                    if (wordDict.ContainsKey(letters))
                    {
                        wordDict[letters].Add(word);
                    }
                    else
                    {
                        List<string> newWordList = new List<string>();
                        newWordList.Add(word);
                        wordDict.Add(letters, newWordList);
                    }
                }

                return wordDict;

            }
            catch (Exception ex)
            {
                return null;
            }
        }


        private Dictionary<string, bool> MakeSubsetDictionary(int wordSize, string letters, Dictionary<string, List<string>> source)
        {
            Dictionary<string, bool> subset = new Dictionary<string, bool>();
            
            foreach(string key in source.Keys)
            {
                if(CanBuild(key, letters))
                {
                    subset.Add(key, true);
                }
            }

            return subset;
        }

        private Dictionary<string, bool> MakeSubsetDictionary(int wordSize, string letters, Dictionary<string, bool> source)
        {
            Dictionary<string, bool> subset = new Dictionary<string, bool>();

            foreach (string key in source.Keys)
            {
                if (CanBuild(key, letters))
                {
                    subset.Add(key, true);
                }
            }

            return subset;
        }

        private string GetRemainingLetters(string target, string source) //input = 2 SORTED strings
        {
            string result = source;
            try
            {            
                int index = 0;
                foreach (char c in target)
                {
                    if (c == result[index])
                    {
                        //remove char at index
                        result = result.Remove(index, 1);
                    }
                    else
                    {
                        while (c != result[index]) //find matching char index
                        {
                            index++;
                            if (index >= result.Length)
                            {
                                break;//should never happen
                            }
                        }
                        if (c == result[index])
                        {
                            //remove char at index
                            result = result.Remove(index, 1);
                        }
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                return result;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {


            wordDict = LoadDictionary("2to8");

            SaveOneWordSizeDictionary(2);
            SaveOneWordSizeDictionary(3);
            SaveOneWordSizeDictionary(4);
            SaveOneWordSizeDictionary(5);
            SaveOneWordSizeDictionary(6);
            SaveOneWordSizeDictionary(7);
            SaveOneWordSizeDictionary(8);
        }

        private string Alphabetize(string s)
        {
            char[] a = s.ToCharArray();
            Array.Sort(a);

            return new string(a);
        }

        private void InitializeSubsetDictionaries(string inputLetters)
        {
            wordDict2 = LoadDictionary("2");
            wordDict3 = LoadDictionary("3");
            wordDict4 = LoadDictionary("4");
            wordDict5 = LoadDictionary("5");
            wordDict6 = LoadDictionary("6");

            sub2 = MakeSubsetDictionary(2, inputLetters, wordDict2);
            sub3 = MakeSubsetDictionary(3, inputLetters, wordDict3);
            sub4 = MakeSubsetDictionary(4, inputLetters, wordDict4);
            sub5 = MakeSubsetDictionary(5, inputLetters, wordDict5);
            sub6 = MakeSubsetDictionary(6, inputLetters, wordDict6);

            subsetDictionaryList.Clear();
            subsetDictionaryList.Add(sub2);//0 (filler)
            subsetDictionaryList.Add(sub2);//1 (filler)
            subsetDictionaryList.Add(sub2);//2
            subsetDictionaryList.Add(sub3);//3
            subsetDictionaryList.Add(sub4);//4
            subsetDictionaryList.Add(sub5);//5
            subsetDictionaryList.Add(sub6);//6

        }
        private void HelpButton_Click(object sender, EventArgs e)
        {
            try
            {
                solutionListBox.Items.Clear();
                solutionList.Clear();
                ClearTrackingNumbers();

                //letters
                string inputLetters = lettersTextBox.Text;
                inputLetters = inputLetters.Trim().ToLower();

                inputLetters = Alphabetize(inputLetters);

                // setup dictionaries
                InitializeSubsetDictionaries(inputLetters);

                //word sizes
                string inputWordSizes = wordSizesTextBox.Text;
                inputWordSizes = inputWordSizes.Trim();
                string[] wordSizeArray = inputWordSizes.Split(' ');
                List<int> wordLengths = new List<int>();

                foreach(string wordSize in wordSizeArray)
                {
                    if (wordSize.Length > 0)
                    {
                        int size = Int32.Parse(wordSize);
                        wordLengths.Add(size);
                    }
                }


                //if (wordLengths.Count <= 1)
                //{
                //    string letters = inputLetters;
                //    if (wordDict.ContainsKey(letters))
                //    {
                //        List<string> wordList = wordDict[letters];

                //        foreach (string clue in wordList)
                //        {
                //            solutionListBox.Items.Add(clue);
                //        }
                //    }
                //    return;
                //}
                //else
                //{


                    List<string> partialSolution = new List<string>();        
                  //  Dictionary<string, bool> subsetDictionary = subsetDictionaryList[wordLengths[0]];
                    Recurse2(inputLetters, wordLengths, 0, partialSolution);

                    //Recurse3(subsetDictionary, inputLetters, wordLengths, 0, partialSolution);

                    /*  old way (too many possibilities)
                    List<char> letters = new List<char>(); //start empty
                    List<char> remainingLetterList = new List<char>();
                    foreach (char c in inputLetters.ToCharArray())
                    {
                        remainingLetterList.Add(c);
                    }
                    Recurse(letters, remainingLetters, wordLengths, currentWordIndex, partialSolution);
                     * */

                    foreach (string solutionString in solutionList)
                    {
                        solutionListBox.Items.Add(solutionString);
                    }                   
                //}
            }
            catch (Exception ex)
            {
                int foo;
                foo = 1;
            }
        }

        private void Recurse2(string remainingLetters, List<int> wordLengths, int currentWordIndex, List<string> partialSolution)
        {
            try
            {
                int wordSize = wordLengths[currentWordIndex];

                Dictionary<string, bool> newSubsetDictionary = MakeSubsetDictionary(wordSize, remainingLetters, subsetDictionaryList[wordSize]);
                foreach (string key in newSubsetDictionary.Keys)
                {
                    List<string> updatedPartialSolution = new List<string>(partialSolution);
                    updatedPartialSolution.Add(key);
                    
                    if (currentWordIndex >= wordLengths.Count - 1) // current location is the last word
                    {
                        AddToSolution(updatedPartialSolution);
                    }
                    else
                    {
                        string newRemainingLetters = GetRemainingLetters(key, remainingLetters);
                        Recurse2(newRemainingLetters, wordLengths, currentWordIndex + 1, updatedPartialSolution);
                    }
                }
            }
            catch (Exception ex)
            {
                int foo;
                foo = 1;
            }
        }


        private void Recurse3(Dictionary<string, bool> subsetDictionary, string remainingLetters, List<int> wordLengths, int currentWordIndex, List<string> partialSolution)
        {
            try
            {
                int wordSize = wordLengths[currentWordIndex];

                foreach (string key in subsetDictionary.Keys)
                {
                    string newRemainingLetters = GetRemainingLetters(key, remainingLetters);
                    
                    Dictionary<string, bool> newSubsetDictionary = MakeSubsetDictionary(wordSize, newRemainingLetters, subsetDictionaryList[wordSize]);

                    List<string> updatedPartialSolution = new List<string>(partialSolution);

                    
                    if (newSubsetDictionary.Count == 0)
                    {
                        continue;
                    }
                    else
                    {
                        Recurse3(newSubsetDictionary, newRemainingLetters, wordLengths, currentWordIndex + 1, updatedPartialSolution);
                    }
                }
            }
            catch (Exception ex)
            {
                int foo;
                foo = 1;
            }
        }

        private void AddToSolution(List<string> list)
        {
            try
            {
                string solutionString = "";
                foreach (string word in list)
                {
                    string key = Alphabetize(word);
                    string value = "";
                    if (wordDict.ContainsKey(key))
                    {
                        List<string> wordList = wordDict[key];
                        value = wordList[0];
                    }

                    solutionString += value;
                    solutionString += " ";
                }

                solutionString = solutionString.Trim();
                bool alreadyASolution = false;
                foreach (string checkString in solutionList)
                {
                    if (solutionString == checkString)
                    {
                        alreadyASolution = true;
                        break;
                    }
                }
                if (!alreadyASolution)
                {
                    solutionList.Add(solutionString);
                }
            }
            catch (Exception ex)
            {
                int foo;
                foo = 1;
            }

        }

        private string CharListToString(List<char> characters)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char character in characters)
            {
                sb.Append(character);
            }
            return sb.ToString();
        }
        private bool MakesAWord(List<char> letters)
        {
            letters.Sort();

            string word = CharListToString(letters);
            
            if (wordDict.ContainsKey(word))
            {
                return true;
            }
            
            return false;
        }

        private bool CanBuild(string target, string source) //input = 2 SORTED strings
        {
            try
            {
                int index = -1;
                foreach (char c in target)
                {
                    index++;
                    if(index >= source.Length)
                    {
                        return false;
                    }
                    while (c != source[index])
                    {
                        index++;
                        if (index >= source.Length)
                        {
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                int foo;
                foo = 1;
            }

            return true;
        }

        private bool HasAVowel(string word)
        {
            foreach (char c in word)
            {
                if(c == 'a' || c == 'e' || c == 'i' || c == 'o' || c == 'u' || c == 'y' )
                {
                    return true;
                }
            }

            return false;

        }

        private void Recurse(List<char> letters, List<char> remainingLetters, List<int> wordLengths, int currentWordIndex, List<string> partialSolution)
        {
            try
            {
                recursiveCalls++;
                if (letters.Count == wordLengths[currentWordIndex]) //have enough letters to make a word
                {
                    if (!MakesAWord(letters))
                    {
                        wordsRejected++;
                        return; //end the search down this line because it doesn't make a word
                    }

                    wordsFound++;

                    List<string> updatedPartialSolution = new List<string>(partialSolution);

                    letters.Sort();
                    updatedPartialSolution.Add(CharListToString(letters));

                    int newCurrentWordIndex = currentWordIndex + 1;
                    if (newCurrentWordIndex < wordLengths.Count) // move to the next word
                    {
                        List<char> emptyList = new List<char>();
                        Recurse(emptyList, remainingLetters, wordLengths, newCurrentWordIndex, updatedPartialSolution);//recursive call for next word
                    }
                    else // globally store possible solution
                    {
                        AddToSolution(updatedPartialSolution);
                        solutionsFound++;
                    }
                }
                else //need more letters to form the next word
                {
                    for (int i = 0; i < remainingLetters.Count; i++)//recursively try each remaining letter
                    {
                        //add 1 more letter
                        List<char> newLetters = new List<char>(letters);
                        newLetters.Add(remainingLetters[i]);
                        lettersAdded++;

                        //remove 1 letter from remaining letters
                        List<char> newRemainingLetters = new List<char>(remainingLetters);
                        newRemainingLetters.RemoveAt(i);

                        Recurse(newLetters, newRemainingLetters, wordLengths, currentWordIndex, partialSolution); //recursive call for next letter
                    }
                }
            }
            catch (Exception ex)
            {
                int foo;
                foo = 1;
            }

        }
        


    }
}

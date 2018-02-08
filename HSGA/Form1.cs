﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Diagnostics;

namespace HSGA
{
    public partial class Form1 : Form
    {
        public string initialDirectory = "C:\\Users\\Elliott\\Documents\\Visual Studio 2017\\Projects\\HSGA\\Assets\\CardsToBeDeserialized";
        public string deckDirectory = "C:\\Users\\Elliott\\Documents\\Visual Studio 2017\\Projects\\metastone-master\\cards" +
            "\\src\\main\\resources\\decks";

        public CardJsonManager JSONHandler;

        public HSGAIndividual GeneIndividual;
        public List<HSGAIndividual> GenePopulation;

        private string selectedClass;

        public Form1()
        {
            InitializeComponent();
            //should init json manager here
            JSONHandler = new CardJsonManager();
            GenePopulation = new List<HSGAIndividual>();
            GeneIndividual = new HSGAIndividual();

           // initialDirectory = Directory.GetCurrentDirectory();
        }

        private void GetAllCards_Click(object sender, EventArgs e)
        {
            //string currentPath = dialog.FileName;
            int numOfFiles = Directory.GetDirectories(initialDirectory, "*", SearchOption.TopDirectoryOnly).Length;

            //deserialize all in directory
            JSONHandler.GetAllCards(numOfFiles, initialDirectory);

            //enable test
            TestValidationButton.Enabled = true;
        }

        private void GenDeckButton_Click(object sender, EventArgs e)
        {
            JSONHandler.filePath = deckDirectory;
            JSONHandler.GenerateRandomDeck();

            NeutralPathLabel.Text = JSONHandler.finalGeneratedDeck;
            label1.Text = JSONHandler.cardCount.ToString();
        }

        private void GenSpecificDeckButton_Click(object sender, EventArgs e)
        {
            selectedClass = comboBox1.GetItemText(comboBox1.SelectedItem);
            JSONHandler.filePath = deckDirectory;
            // JSONHandler.GenerateSpecificDeck(selectedClass);
            GeneIndividual.deck = JSONHandler.GenerateSpecificDeck(selectedClass);
            NeutralPathLabel.Text = JSONHandler.finalGeneratedDeck;
            label1.Text = JSONHandler.cardCount.ToString();
        }

        private void TestValidationButton_Click(object sender, EventArgs e)
        {
           label2.Text =  JSONHandler.TestValidation();
        }

        private void GetAllSelectedCards_Click(object sender, EventArgs e)
        {
            //CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            //dialog.InitialDirectory = initialDirectory;
            //dialog.IsFolderPicker = true;

            int numOfFiles = Directory.GetDirectories(initialDirectory, "*", SearchOption.TopDirectoryOnly).Length;

            selectedClass = comboBox1.GetItemText(comboBox1.SelectedItem);
            string currentPath = initialDirectory + "\\" + selectedClass;

            //deserialize all in directory
            JSONHandler.GetAllSelectedCards(currentPath, selectedClass);
        }

        private void GenButton_Click(object sender, EventArgs e)
        {
            //Using a process object to allow for multiple commands to be 
            //input into the command line process.
            Process p = new Process();
            ProcessStartInfo info = new ProcessStartInfo();
            info.FileName = "CMD.exe";
            info.RedirectStandardInput = true;
            info.UseShellExecute = false;

            p.StartInfo = info;
            p.Start();

            using (StreamWriter sw = p.StandardInput)
            {
                if (sw.BaseStream.CanWrite)
                {
                    sw.WriteLine("cd C:\\Users\\Elliott\\Desktop\\DissertationProjects2017_18\\metastone-master");
                    sw.WriteLine("gradlew run");
                }
            }
        }
    }
}

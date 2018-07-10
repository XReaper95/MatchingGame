using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MatchingGame
{
    public partial class Form1 : Form
    {
        //constructor
        public Form1()
        {
            InitializeComponent();
            AssingIconsToSquares();
        }

        //random object
        Random random = new Random();

        //hardcode interrogation mark ( "s" in webdings font)
        string unknownIcon = "s";

        //label references
        Label firstClicked = null;
        Label secondClicked = null;

        // Each of these letters is an interesting icon
        // in the Webdings font,
        // and each icon appears twice in this list
        List<string> icons = new List<string>()
        {
        "!", "!", "N", "N", ",", ",", "k", "k",
        "b", "b", "v", "v", "w", "w", "z", "z"
        };

        //stores the assigned random value for the labels
        Dictionary<Label, string> labelValues = new Dictionary<Label, string>();

        private void AssingIconsToSquares()
        {
            foreach (Control control in tableLayoutPanel1.Controls)
            {
                Label iconLabel = control as Label;
                if (iconLabel != null)
                {
                    int randomNumber = random.Next(icons.Count);
                    labelValues.Add(iconLabel, icons[randomNumber]);
                    icons.RemoveAt(randomNumber);
                }
            }
        }

        /// <summary>
        /// Every label's Click event is handled by this event handler
        /// </summary>
        /// <param name="sender">The label that was clicked</param>
        /// <param name="e"></param>
        private void label_Click(object sender, EventArgs e)
        {
            Label clickedLabel = sender as Label;
            if (clickedLabel != null)
            {
                // The timer is only on after two non-matching 
                // icons have been shown to the player, 
                // so ignore any clicks if the timer is running
                if (timer1.Enabled == true)
                    return;

                // If the clicked label is visible, the player clicked
                // an icon that's already been revealed --
                // ignore the click
                if (clickedLabel.Text != unknownIcon) return;

                // If firstClicked is null, this is the first icon 
                // in the pair that the player clicked,
                // so set firstClicked to the label that the player 
                // clicked, change its color to black, and return
                if (firstClicked == null)
                {
                    firstClicked = clickedLabel;
                    clickedLabel.Text = labelValues[clickedLabel];
                    return;
                }

                //Otherwise, this is the second icon
                //in the pair, so secondClicked equals
                //this icon, and show icon
                secondClicked = clickedLabel;
                clickedLabel.Text = labelValues[clickedLabel];

                //check if the user has won
                CheckForWinner();

                // If the player clicked two matching icons, keep them 
                // visible and reset firstClicked and secondClicked 
                // so the player can click another icon
                if (firstClicked.Text == secondClicked.Text)
                {
                    firstClicked = null;
                    secondClicked = null;
                    return;
                }

                // If the player gets this far, the player 
                // clicked two different icons, so start the 
                // timer (which will wait three quarters of 
                // a second, and then hide the icons)
                timer1.Start();
            }
        }

        /// <summary>
        /// This timer is started when the player clicks 
        /// two icons that don't match,
        /// so it counts three quarters of a second 
        /// and then turns itself off and hides both icons
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            //stop timer
            timer1.Stop();

            //hide icons
            firstClicked.Text = unknownIcon;
            secondClicked.Text = unknownIcon;

            //reset the clicked icons
            firstClicked = null;
            secondClicked = null;
        }

        /// <summary>
        /// Check every icon to see if it is matched, by 
        /// comparing its foreground color to its background color. 
        /// If all of the icons are matched, the player wins
        /// </summary>
        private void CheckForWinner()
        {
            // Go through all of the labels in the TableLayoutPanel, 
            // checking each one to see if its icon is matched
            foreach (Control control in tableLayoutPanel1.Controls)
            {
                Label iconLabel = control as Label;

                if (iconLabel != null)
                {
                    if (iconLabel.Text == unknownIcon)
                        return;
                }
            }

            // If the loop didn't return, it didn't find
            // any unmatched icons
            // That means the user won. Show a message and close the form
            MessageBox.Show("You matched all the icons!", "Congratulations");
            Close();
        }
    }
}


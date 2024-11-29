using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace VoiceAnimationXamarin
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void StartAnimation_Clicked(object sender, EventArgs e)
        {
            BarAnimationView.StartAnimation(); // Start the animation
        }

        private void StopAnimation_Clicked(object sender, EventArgs e)
        {
            BarAnimationView.StopAnimation(); // Stop the animation
        }
    }
}

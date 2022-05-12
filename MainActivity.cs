using Android.App;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Widget;
using AndroidX.AppCompat.App;
using AndroidX.ConstraintLayout.Widget;
using AndroidX.Palette.Graphics;

namespace com.companyname.ColorPalette
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity, Palette.IPaletteAsyncListener
    {
        
        private ConstraintLayout constraintLayout;
        private TextView textViewTitle;
        private TextView textViewBody;
        private ImageView imageView;
        private Button buttonNextSwatch;

        private TextView textViewBackgroundColor;
        private TextView textViewTitleTextColor;
        private TextView textViewBodyTextColor;

        private TextView textViewBackgroundColorAlpha;
        private TextView textViewTitleTextColorAlpha;
        private TextView textViewBodyTextColorAlpha;
            

        private Palette.Swatch vibrantSwatch;
        private Palette.Swatch lightVibrantSwatch;
        private Palette.Swatch darkVibrantSwatch;
        private Palette.Swatch mutedSwatch;
        private Palette.Swatch lightMutedSwatch;
        private Palette.Swatch darkMutedSwatch;
        
        // It looks like Dominant was added later, which is a swatch with the highest population. see Swatch.Population. Would appear to be very similar to DarkVibrant.
        //private Palette.Swatch dominantSwatch;

        private int swatchNumber = 0;

        // Interesting discussion
        //https://stackoverflow.com/questions/28144847/differences-between-android-palette-colors/28776080
        //https://chris.banes.dev/palette-v21/
        // Interesting layout
        //https://github.com/hanmajid/modern-android-samples/blob/master/palette/app/src/main/res/layout/activity_main.xml

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            constraintLayout = FindViewById<ConstraintLayout>(Resource.Id.constraint);
            textViewTitle = FindViewById<TextView>(Resource.Id.text_view_title);
            textViewBody = FindViewById<TextView>(Resource.Id.text_view_body);
            
            imageView = FindViewById<ImageView>(Resource.Id.image_view);
            buttonNextSwatch = FindViewById<Button>(Resource.Id.button_next_swatch);
            Bitmap bitmap = ((BitmapDrawable)imageView.Drawable).Bitmap;

            textViewBackgroundColor = FindViewById<TextView>(Resource.Id.text_view_background_color);
            textViewTitleTextColor = FindViewById<TextView>(Resource.Id.text_view_title_text_color);
            textViewBodyTextColor = FindViewById<TextView>(Resource.Id.text_view_body_text_color);
            textViewBackgroundColorAlpha = FindViewById<TextView>(Resource.Id.text_view_backgroundcolor_alpha);
            textViewTitleTextColorAlpha = FindViewById<TextView>(Resource.Id.text_view_title_text_color_alpha);
            textViewBodyTextColorAlpha = FindViewById<TextView>(Resource.Id.text_view_body_text_color_alpha);


            // Palette.IPaletteAsyncListener. Generates Palette on a background thread.
            Palette.From(bitmap).MaximumColorCount(32).Generate(this);
            
            buttonNextSwatch.Click += NextSwatch_Click;
        }

        #region OnGenerated
        public void OnGenerated(Palette palette)
        {
            vibrantSwatch = palette.VibrantSwatch;
            lightVibrantSwatch = palette.LightVibrantSwatch;
            darkVibrantSwatch = palette.DarkVibrantSwatch;
            mutedSwatch = palette.MutedSwatch;
            lightMutedSwatch = palette.LightMutedSwatch;
            darkMutedSwatch = palette.DarkMutedSwatch;


            //dominantSwatch = palette.DominantSwatch;
        }
        #endregion

        #region NextSwatch Click Event
        private void NextSwatch_Click(object sender, System.EventArgs e)
        {
            Palette.Swatch currentSwatch = null;
            switch (swatchNumber)
            {
                case 0:
                    currentSwatch = vibrantSwatch;
                    textViewTitle.Text = "Vibrant";
                    break;

                case 1:
                    currentSwatch = lightVibrantSwatch;
                    textViewTitle.Text = "Light Vibrant"; 
                    break;

                case 2:
                    currentSwatch = darkVibrantSwatch;
                    textViewTitle.Text = "Dark Vibrant"; 
                    break;

                // My testing shows darkVibrantSwatch values are the same as dominantSwatch values
                //case 3:
                //    currentSwatch = dominantSwatch;
                //    textViewTitle.Text = "Dominant";
                //    break;

                case 3:
                    currentSwatch = mutedSwatch;
                    textViewTitle.Text = "Muted";
                    break;

                case 4:
                    currentSwatch = lightMutedSwatch;
                    textViewTitle.Text = "Light Muted";
                    break;

                case 5:
                    currentSwatch = darkMutedSwatch;
                    textViewTitle.Text = "Dark Muted";
                    break;
            }

            if (currentSwatch != null)
            {
                
                constraintLayout.SetBackgroundColor(new Color(currentSwatch.Rgb));
                textViewTitle.SetTextColor(new Color(currentSwatch.TitleTextColor));
                
                // Get the BodyTextColor, so we can set the following TextViews. 
                Color bodyTextColor = new Color(currentSwatch.BodyTextColor);
                
                textViewBody.SetTextColor(bodyTextColor);
                textViewBackgroundColor.SetTextColor(bodyTextColor);
                textViewBackgroundColorAlpha.SetTextColor(bodyTextColor);

                textViewTitleTextColor.SetTextColor(bodyTextColor);
                textViewTitleTextColorAlpha.SetTextColor(bodyTextColor);

                textViewBodyTextColor.SetTextColor(bodyTextColor);
                textViewBodyTextColorAlpha.SetTextColor(bodyTextColor);

                textViewBackgroundColor.Text = "BackgroundColor: "  + string.Format("#{0:x}", currentSwatch.Rgb);
                textViewBackgroundColorAlpha.Text = "Alpha value: " + string.Format("{0:f1}%", (Color.GetAlphaComponent(currentSwatch.Rgb) / 255.0) * 100);

                textViewTitleTextColor.Text = "TitleTextColor: "    + string.Format("#{0:x}", currentSwatch.TitleTextColor); 
                textViewTitleTextColorAlpha.Text = "Alpha value: "  + string.Format("{0:f1}%", (Color.GetAlphaComponent(currentSwatch.TitleTextColor) / 255.0) * 100);

                textViewBodyTextColor.Text = "BodyTextColor: "      + string.Format("#{0:x}", currentSwatch.BodyTextColor);
                textViewBodyTextColorAlpha.Text = "Alpha value: "   + string.Format("{0:f1}%", (Color.GetAlphaComponent(currentSwatch.BodyTextColor) / 255.0) * 100);

                Window.SetNavigationBarColor(new Color(currentSwatch.Rgb));
            }
            else
            {
                // Swatch not found
                constraintLayout.SetBackgroundColor(Color.White);
                textViewTitle.SetTextColor(Color.Red);
                textViewBody.SetTextColor(Color.Red);
            }

            if (swatchNumber < 5)
                swatchNumber++;
            else
                swatchNumber = 0;
        }
        #endregion

    }
}
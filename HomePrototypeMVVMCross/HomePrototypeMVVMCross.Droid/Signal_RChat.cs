using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNet.SignalR.Client;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Graphics;

namespace HomePrototypeMVVMCross.Droid
{
    [Activity(Label = "")]
    public class Signal_RChat : Activity
    {

        public string UserName;
        public int BackgroundColor;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.signal_RChat);

            GettingInformations getInfo = new GettingInformations();
            getInfo.OnGetInfoComplete += GettingInformations_OnGetInfoComplete;
            getInfo.Show(FragmentManager, "GettingInformations");
        }

        private async void GettingInformations_OnGetInfoComplete(object sender, GettingInformations.OnGetInfoCompletEventArgs e)
        {
            UserName = e.TxtName;
            BackgroundColor = e.BackgroundColor;

            var hubConnection = new HubConnection("http://192.158.5.8"); //must set up your own local host
            var chatHubProxy = hubConnection.CreateHubProxy("ChatHub");

            chatHubProxy.On<string, int, string>("UpdateChatMessage", (message, color, user) =>
            {
                //UpdateChatMessage has been called from server

                RunOnUiThread(() =>
                {
                    TextView txt = new TextView(this);
                    txt.Text = user + ": " + message;
                    txt.SetTextSize(Android.Util.ComplexUnitType.Sp, 20);
                    txt.SetPadding(10, 10, 10, 10);

                    switch (color)
                    {
                        case 1:
                            txt.SetTextColor(Color.Red);
                            break;

                        case 2:
                            txt.SetTextColor(Color.DarkGreen);
                            break;

                        case 3:
                            txt.SetTextColor(Color.Blue);
                            break;

                        default:
                            txt.SetTextColor(Color.Black);
                            break;

                    }

                    txt.LayoutParameters = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent)
                    {
                        TopMargin = 10,
                        BottomMargin = 10,
                        LeftMargin = 10,
                        RightMargin = 10,
                        Gravity = GravityFlags.Right
                    };

                    FindViewById<LinearLayout>(Resource.Id.llChatMessages)
                            .AddView(txt);
                });
            });

            try
            {
                await hubConnection.Start();
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            FindViewById<Button>(Resource.Id.btnSend).Click += async (o, e2) =>
            {
                var message = FindViewById<EditText>(Resource.Id.txtChat).Text;

                await chatHubProxy.Invoke("SendMessage", new object[] { message, BackgroundColor, UserName });
            };

        }
    }
}
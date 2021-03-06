﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using libSnarlStyles;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using BeezleTester;
using System.Text.RegularExpressions;

namespace hover
{
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.AutoDual)]

    public class StyleInstance : IStyleInstance
    {
        private BeezleDisplay myDisplay;

        public StyleInstance()
        {
            
        }

         

        #region IStyleInstance Members

        [ComVisible(true)]
        void IStyleInstance.AdjustPosition(ref int x, ref int y, ref short Alpha, ref bool Done)
        {
            x = Screen.PrimaryScreen.WorkingArea.Height / 2;
            y = Screen.PrimaryScreen.WorkingArea.Width / 2;
        }

        [ComVisible(true)]
        melon.MImage IStyleInstance.GetContent()
        {
          //  MessageBox.Show("GetContent");
            return null;
            throw new NotImplementedException();
        }

        [ComVisible(true)]
        bool IStyleInstance.Pulse()
        {
            return false;
            throw new NotImplementedException();
        }

        [ComVisible(true)]
        void IStyleInstance.Show(bool Visible)
        {
           // MessageBox.Show("Show: " + Visible.ToString());
        }

        [ComVisible(true)]
        void IStyleInstance.UpdateContent(ref notification_info NotificationInfo)
        {
            
            // MessageBox.Show("UpdateContent");
            try
            {
                if (myDisplay == null)
                {
                    myDisplay = new BeezleDisplay();
                }
                if (myDisplay.isClosing)
                {
                    myDisplay.stopClosing();
                }
                myDisplay.setNewIconPath(NotificationInfo.Icon);
                myDisplay.setPriority(false);
                if (NotificationInfo.Flags == S_NOTIFICATION_FLAGS.S_NOTIFICATION_IS_PRIORITY)
                {
                    myDisplay.setPriority(true);
                }



                switch (NotificationInfo.Scheme)
                {
                    case "icon only":
                        myDisplay.showIconOnly();
                        break;

                    case "title":
                        myDisplay.showText(NotificationInfo.Title);
                        break;

                    case "text":
                        myDisplay.showText(NotificationInfo.Text);
                        break;

                    case "meter":
                        myDisplay.showProgressBar(parseTextForPercentage(NotificationInfo.Text));
                        break;

                    default:
                        myDisplay.showIconOnly();
                        break;
                }

                myDisplay.startTimer(5);
            }
            catch
            {
                Console.WriteLine("Mist");
            }
            
        }


        private int parseTextForPercentage(string text)
        {
            Regex onlyDigits = new Regex("^[0-9]{1,3}$");
            if (onlyDigits.IsMatch(text))
            {
                return Convert.ToInt32(text);
            }

            Regex withPercentageSymbol = new Regex(@"(?<percentage>[0-9]{1,3}) {0,1}%");
            if (withPercentageSymbol.IsMatch(text))
            {
                Match myMatch = withPercentageSymbol.Match(text);
                return Convert.ToInt32(myMatch.Groups["percentage"].ToString());
            }

            Regex withPercentageText = new Regex(@"(?<percentage>[0-9]{1,3}) {0,1}Percent", RegexOptions.IgnoreCase);
            if (withPercentageText.IsMatch(text))
            {
                Match myMatch = withPercentageText.Match(text);
                return Convert.ToInt32(myMatch.Groups["percentage"].ToString());
            }

            Regex withPercentageMath = new Regex(@"(?<percentage>[0-9]{1,3}) {0,1}\/ {0,1}100", RegexOptions.IgnoreCase);
            if (withPercentageMath.IsMatch(text))
            {
                Match myMatch = withPercentageMath.Match(text);
                return Convert.ToInt32(myMatch.Groups["percentage"].ToString());
            }
            return 0;
        }

        #endregion


    }
}
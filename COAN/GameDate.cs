using System;
using System.Collections.Generic;

namespace COAN
{
    public class GameDate
    {
        private readonly Dictionary<int, string> months = new Dictionary<int, string>();
        public int year = 0;
        public int month = 0;
        public int day = 0;
        public long date;

        public GameDate(long date)
        {
            int rem;
            months.Add(1, "January");
            months.Add(2, "Feburary");
            months.Add(3, "March");
            months.Add(4, "April");
            months.Add(5, "May");
            months.Add(6, "June");
            months.Add(7, "July");
            months.Add(8, "August");
            months.Add(9, "September");
            months.Add(10, "October");
            months.Add(11, "November");
            months.Add(12, "December");

            /* There are 97 leap years in 400 years */
            year = (int)(400 * Math.Floor((double)date / (365 * 400 + 97)));
            rem = (int)(date % (365 * 400 + 97));

            /* There are 24 leap years in 100 years */
            year += (int)(100 * Math.Floor((double)rem / (365 * 100 + 24)));
            rem = (int)(rem % (365 * 100 + 24));

            /* There is 1 leap year every 4 years */
            year += (int)(4 * Math.Floor((double)rem / (365 * 4 + 1)));
            rem = (int)(rem % (365 * 4 + 1));

            while (rem >= (isLeapYear(year) ? 366 : 365))
            {
                rem -= isLeapYear(year) ? 366 : 365;
                year++;
            }

            if (!isLeapYear(year) && rem >= 30 + 28) rem++;

            day = rem;
            setMonthDay();
        }

        public bool isLeapYear(double year)
        {
            return year % 4 == 0 && (year % 100 != 0 || year % 400 == 0);
        }

        public void setMonthDay()
        {
            if (day > 31)
            {
                month++;
                day -= 31;
            }

            if (day > 29)
            {
                month++;
                day -= 29;
            }

            if (day > 31)
            {
                month++;
                day -= 31;
            }

            if (day > 30)
            {
                month++;
                day -= 30;
            }

            if (day > 31)
            {
                month++;
                day -= 31;
            }

            if (day > 30)
            {
                month++;
                day -= 30;
            }

            if (day > 31)
            {
                month++;
                day -= 31;
            }

            if (day > 31)
            {
                month++;
                day -= 31;
            }

            if (day > 30)
            {
                month++;
                day -= 30;
            }

            if (day > 31)
            {
                month++;
                day -= 31;
            }

            if (day > 30)
            {
                month++;
                day -= 30;
            }

            if (day > 31)
            {
                month++;
                day -= 31;
            }

            month++;
        }

        public int getYear()
        {
            return year;
        }

        public int getMonth()
        {
            return month;
        }

        public int getDay()
        {
            return day;
        }
        /**
         * @return Standard date in Year-Month-Date format.
         */
        public string toString()
        {
            day++;
            return day + "-" + months[month] + "-" + year;
        }
    }
}

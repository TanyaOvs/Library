using System;

namespace Library
{
    public class Book
    {
        private string title;
        private string author;
        private string genre;
        private double collateralValue;
        private double rentalCost;

        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        public string Author
        {
            get { return author; }
            set { author = value; }
        }

        public string Genre
        {
            get { return genre; }
            set { genre = value; }
        }

        public double CollateralValue
        {
            get { return collateralValue; }
            set { collateralValue = value; }
        }

        public double RentalCost
        {
            get { return rentalCost; }
            set { rentalCost = value; }
        }

        public Book(string _title, string _author, string _genre, double _collateral_value, double _rental_cost) 
        {
            Title = _title;
            Author = _author;
            Genre = _genre;
            CollateralValue = _collateral_value;
            RentalCost = _rental_cost;
        }

    }
}

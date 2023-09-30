using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Clothes
{
    [Serializable]
    public class ClothingModel : INotifyPropertyChanged
    {
        private string clothingType = "New Clothing";
        /// <summary>
        /// The type description of the item of clothing (e.g. jumper)
        /// </summary>
        public string ClothingType
        {
            get { return clothingType; }
            set
            {
                clothingType = value;
                RaisePropertyChanged("ClothingType");
            }
        }

        private string colour;
        /// <summary>
        /// The colour of the item of clothing
        /// </summary>
        public string Colour
        {
            get { return colour; }
            set
            {
                colour = value;
                RaisePropertyChanged("Colour");
            }
        }

        private string condition;
        /// <summary>
        /// The condition of the item of clothing
        /// </summary>
        public string Condition
        {
            get { return condition; }
            set
            {
                condition = value;
                RaisePropertyChanged("Condition");
            }
        }

        private string notes;
        /// <summary>
        /// Notes about the item of clothing
        /// </summary>
        public string Notes
        {
            get { return notes; }
            set { notes = value; RaisePropertyChanged("Notes"); }
        }

        private string acquisitionDate;
        /// <summary>
        /// The date on which the item of clothing was acquired
        /// </summary>
        public string AcquisitionDate
        {
            get { return acquisitionDate; }
            set { acquisitionDate = value; RaisePropertyChanged("AcquisitionDate"); }
        }

        private bool archived;
        /// <summary>
        /// True if the item has been archived
        /// </summary>
        public bool Archived
        {
            get { return archived; }
            set { archived = value; RaisePropertyChanged("AcquisitionDate"); }
        }

        private string cost;
        /// <summary>
        /// Stores the cost of the item
        /// </summary>
        public string Cost
        {
            get { return cost; }
            set { cost = value; RaisePropertyChanged("AcquisitionDate"); }
        }

        private bool exactCost;
        /// <summary>
        /// True if the exact cost of the item is known
        /// </summary>
        public bool ExactCost
        {
            get { return exactCost; }
            set { exactCost = value; }
        }

        public List<string> ImageFilepaths = new List<string>();

        /// <summary>
        /// Determine how the object appears in debugging
        /// </summary>
        /// <returns>The ToString default for the object</returns>
        public override string ToString()
        {
            return $"{ClothingType}";
        }

        /// <summary>
        /// Paramaterless constructor required for serialisation
        /// </summary>
        public ClothingModel() {}

        protected void RaisePropertyChanged(string propertyName) { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)); }
        [field: NonSerializedAttribute]
        public event PropertyChangedEventHandler PropertyChanged;
    }
}

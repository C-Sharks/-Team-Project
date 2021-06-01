using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interpol
{
    /// <summary>
    /// Класс для удобной загнрузки данных с базу в датагрид
    /// </summary>
    class GridCriminal
    {
        public GridCriminal(Сriminals obj)
        {
            Id = obj.Id;
            FirstName = obj.FirstName;
            LastName = obj.LastName;
            Patronymic = obj.Patronymic;
            Gender = obj.Gender;
            Birthday = obj.Birthday.ToShortDateString();
            PlaceOfBirth = obj.PlaceOfBirth;
            Languages = obj.Languages;
            Status = obj.Status;
            Nationality = obj.Nationality;
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Patronymic { get; set; }
        public string Gender { get; set; }
        public string Birthday { get; set; }
        public string PlaceOfBirth { get; set; }
        public string Languages { get; set; }
        public string Status { get; set; }
        public string Nationality { get; set; }
    }
}

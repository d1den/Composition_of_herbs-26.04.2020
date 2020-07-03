using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prog1
{
    /// <summary>
    /// Класс для хранения компонентов травяного сбора
    /// </summary>
    class Components
    {
        public string name; // Название травы
        public double persent=0.0; // Процент содержания травы
        public double weight=0.0; // Вес травы
        public Components(string Name, double pers) // Конструктор класса
        {
            name = Name; // Записываем имя
            persent = pers; // Записываем процент
            weight = Herbs.allWeight * persent / 100.0; // Рассчитываем массу травы
        }
    }
}

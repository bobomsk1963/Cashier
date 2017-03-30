using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;



// Добавить пакет NuGet - EntityFramework.SqlServerCompact  версия 6.1.3
namespace InterfaceCashier
{
    public class cNomenklatura
    {
        //[ForeignKey("аааId")]
        [Key]
        [ReadOnly(true)]
        [DisplayName("1. Код")]
        [Description("Код товара")]
        public int Id { get; set; }
        [Required]            // Определяет что поле не пустое
        [StringLength(100)]
        [DisplayName("2. Наименование")]
        [Description("Наименование товара")]
        public string Name { get; set; }
        [StringLength(20)]
        [DisplayName("3. Штрих-код")]
        [Description("Штрих-код товара")]
        public string ShtrihCode { get; set; }
        [StringLength(20)]
        [DisplayName("4. Артикул")]
        [Description("Артикул товара")]
        public string Articl { get; set; }
        [DisplayName("5. Цена")]
        [Description("Цена товара")]
        public decimal Price { get; set; }  // Стоимость
        [Browsable(false)]
        public virtual ICollection<cElementPost> ElementPost { get; set; }
        [Browsable(false)]
        public virtual ICollection<cElementSale> ElementSale { get; set; }
        //public cNomenklatura()
        //{
        //    ElementPost = new List<cElementPost>();
        //    ElementSale = new List<cElementSale>();
        //}
        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public override string ToString()
        {
            return //ShtrihCode+"/"+
                Name+"/"+Articl;//base.ToString();
        }
    }

    public class cPost
    {
        [Key]
        [ReadOnly(true)]
        [DisplayName("1. Код")]
        [Description("Код поступления")]
        public int Id { get; set; }
        [ReadOnly(true)]
        [Required]
        [DisplayName("2. Номер накладной")]
        //[Description("Код товара")]
        public int Nuber { get; set; }
        [Required]
        [DisplayName("3. Дата")]
        public DateTime dateTime { get; set; }
        [Browsable(false)]
        public virtual ICollection<cElementPost> ElementPost { get; set; }
        //public cPost()
        //{
        //    ElementPost = new List<cElementPost>();
        //}

    }

    public class cElementPost
    {
        [Key]
        [DisplayName("Код")]
        //[Description("Код элемента поступления")]
        public int Id { get; set; }
        [Browsable(false)]
        public int NomenklaturaId { get; set; }
        [DisplayName("Товар")]
        public virtual cNomenklatura Nomenklatura { get; set; }
        [Browsable(false)]
        public int PostId { get; set; }
        [Browsable(false)]
        public virtual cPost Post { get; set; }
        [DisplayName("Цена")]
        [Description("Цена товара")]
        public decimal Price { get; set; }
        [DisplayName("Количество")]
        [Description("Количество товара")]
        public int Count { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }

    public class cSale
    {
        [Key]
        [ReadOnly(true)]
        [DisplayName("1. Код")]
        [Description("Код продажи")]
        public int Id { get; set; }
        [Required]
        [ReadOnly(true)]
        [DisplayName("2. Номер продажи")]
        public int Number { get; set; }
        [Required]
        [ReadOnly(true)]
        [DisplayName("3. Дата и время продажи")]
        public DateTime dateTime { get; set; }
        [ReadOnly(true)]
        [DisplayName("4. Тип продажи")]
        public int TypeSale { get; set; }
        [ReadOnly(true)]
        [DisplayName("5. Деньги клиента")]
        public decimal ManyClient { get; set; }
        [ReadOnly(true)]
        [DisplayName("6. Сдача")]
        public decimal ManyChange { get; set; }

        [Browsable(false)]
        public virtual ICollection<cElementSale> ElementSale { get; set; }
        //public cSale()
        //{
        //    ElementSale = new List<cElementSale>();
        //}

    }



    public class cElementSale
    {
        [Key]
        [DisplayName("Код")]
        [Description("Код элемента продажи")]
        public int Id { get; set; }
        [Browsable(false)]
        public int NomenklaturaId { get; set; }
        public virtual cNomenklatura Nomenklatura { get; set; }
        [Browsable(false)]
        public int SaleId { get; set; }
        [Browsable(false)]
        public virtual cSale Sale { get; set; }
        [DisplayName("Цена")]
        [Description("Цена товара")]
        public decimal Price { get; set; } //Цена 
        [DisplayName("Количество")]
        [Description("Количество товара")]
        public int Count { get; set; }
        [DisplayName("Скидка")]
        public decimal Discount { get; set; }
             
    }


    class MyContextInitializer : DropCreateDatabaseAlways<CashierContext> // Для автоматического пересоздания  в случае изменения модели
    {
       //  Для автоматического добавлении данных для пересоздании 
       // protected override void Seed(MobileContext db) 
       // {
       //     Phone p1 = new Phone { Name = "Samsung Galaxy S5", Price = 14000 };
       //     db.Phones.Add(p1);
       //     db.SaveChanges();
       // }
    }


    public class CashierContext : DbContext
    {
        // Имя будущей базы данных можно указать через
        // вызов конструктора базового класса
        public CashierContext()
            : base("InterfaceCashier")
        {
            //Database.SetInitializer<CashierContext>(new MyContextInitializer()); // Для автоматического пересоздания  в случае изменения модели
        }
        
        // Отражение таблиц базы данных на свойства с типом DbSet
        public DbSet<cNomenklatura> Nomenklatura { get; set; }
        public DbSet<cPost> Post { get; set; }
        public DbSet<cElementPost> ElementPost { get; set; }        
        public DbSet<cSale> Sale { get; set; }
        public DbSet<cElementSale> ElementSale { get; set; }
    }
}

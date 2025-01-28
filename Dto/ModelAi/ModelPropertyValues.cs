using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dto.ModelAi
{
    public class ModelPropertyValues
    {
        public List<string> UsageCount { get; set; }
        public List<string> ModelImageUrl { get; set; }
        public List<string> Type { get; set; }
        public List<string> Voice { get; set; }
        public List<string> Language { get; set; }
        public List<string> Dialect { get; set; }
        public List<string> Quality { get; set; }
        public List<string> Token { get; set; }
        public List<string> ModelVersion { get; set; }
        public List<string> CreationDate { get; set; }
        public List<string> LastUpdated { get; set; }
        public List<string> Description { get; set; }
        public List<string> Author { get; set; }
        public List<string> Accuracy { get; set; }
        public List<string> Speed { get; set; }
        public List<string> Framework { get; set; }
        public List<string> Parameters { get; set; }
    


    public ModelPropertyValues GetEnglishPropertyValues()
        {
            return new ModelPropertyValues
            {
                UsageCount = new List<string> { "600", "8888" },
                ModelImageUrl = new List<string> { "/ai-hand.png", "/ai-robot.png", "chat-boat.jpg", "chatbot-cta.png" },
                Type = new List<string> { "Text To Text", "Text To Speech", "Chat Model" },
                Voice = new List<string> { "Male", "Female" },
                Language = new List<string> { "English", "German", "Arabic" },
                Dialect = new List<string> { "Najd", "Hijazi" },
                Quality = new List<string> { "High", "Medium", "Low" },
                Token = new List<string> { "100", "500", "1000", "5000", "10000" },
                ModelVersion = new List<string> { "v1.0", "v1.1", "v2.0", "v2.5", "v3.0" },
                CreationDate = new List<string> { "2023-01-01", "2022-12-15", "2021-11-20" },
                LastUpdated = new List<string> { "2023-12-01", "2023-11-15", "2023-10-30" },
                Description = new List<string> { "Random generated description.", "High-quality model for tasks.", "Used in various applications." },
                Author = new List<string> { "Heba V1", "Heba V2", "Heba V3", "BssamV1", "BssamV2" },
                Accuracy = new List<string> { "0.90", "0.85", "0.95" },
                Speed = new List<string> { "Fast", "Medium", "Slow" },
                Framework = new List<string> { "TensorFlow", "PyTorch", "Keras" },
                Parameters = new List<string> { "1000000", "5000000", "10000000" }
            };
        }


        public ModelPropertyValues GetArabicPropertyValues()
        {
            return new ModelPropertyValues
            {
                UsageCount = new List<string> { "600", "8888" },
                ModelImageUrl = new List<string> { "/ai-hand.png", "/ai-robot.png", "chat-boat.jpg", "chatbot-cta.png" },
                Type = new List<string> { "نص إلى نص", "نص إلى كلام", "نموذج محادثة" },
                Voice = new List<string> { "ذكر", "أنثى" },
                Language = new List<string> { "إنجليزي", "صيني", "عربي" },
                Dialect = new List<string> { "نجدي", "حجازي" },
                Quality = new List<string> { "عالي", "متوسط", "منخفض" },
                Token = new List<string> { "100", "500", "1000", "5000", "10000" },
                ModelVersion = new List<string> { "v1.0", "v1.1", "v2.0", "v2.5", "v3.0" },
                CreationDate = new List<string> { "2023-01-01", "2022-12-15", "2021-11-20" },
                LastUpdated = new List<string> { "2023-12-01", "2023-11-15", "2023-10-30" },
                Description = new List<string> { "وصف تم إنشاؤه عشوائيًا.", "نموذج عالي الجودة للمهام.", "يستخدم في العديد من التطبيقات." },
                Author = new List<string> { "النموذج توليف 1", "النموذج توليف  2", "النموذج توليف 3" },
                Accuracy = new List<string> { "0.90", "0.85", "0.95" },
                Speed = new List<string> { "سريع", "متوسط", "بطيء" },
                Framework = new List<string> { "TensorFlow", "PyTorch", "Keras" },
                Parameters = new List<string> { "1000000", "5000000", "10000000" }
            };
        }

        public ModelPropertyValues GetPropertyValues(bool isEnglish)
        {
            if (isEnglish)
            {
                return GetEnglishPropertyValues();
            }
            else
            {
                return GetArabicPropertyValues();
            }
        }


    }
    
   
    public class DataFilter
    {
        public ValueFilterModel GetData(string lg)
        {

            ValueFilterModel valueFilterModel = null;

            if (lg == "en")
            {
                var Type = new List<FilterModelPropertyValues>()
           {
               new FilterModelPropertyValues()
               {
                   Id=Guid.NewGuid().ToString(),
                   Name="Text2Text",Sender="T2T",Forginkey="Categorys"
               },
                 new FilterModelPropertyValues()
               {
                   Id=Guid.NewGuid().ToString(),
                   Name="Text2Speech",Sender="T2S",Forginkey="Categorys"
               },
                  new FilterModelPropertyValues()
               {
                   Id=Guid.NewGuid().ToString(),
                   Name="Chat",Sender="Chat",Forginkey="Categorys"
               },


           };

                var Categorys = new List<FilterModelPropertyValues>()
   {
       new FilterModelPropertyValues()
       {
           Id=Guid.NewGuid().ToString(),
           Name="News",Sender="News",Forginkey="Categorys"
       },
         new FilterModelPropertyValues()
       {
           Id=Guid.NewGuid().ToString(),
           Name="general",Sender="general",Forginkey="Categorys"
       }

   };
                var Langague = new List<FilterModelPropertyValues>()
   {
        new FilterModelPropertyValues()
       {
           Id=Guid.NewGuid().ToString(),
           Name="English",Sender="English",Forginkey="Categorys"
       },
         new FilterModelPropertyValues()
       {
           Id=Guid.NewGuid().ToString(),
           Name="Arabic",Sender="Arabic",Forginkey="Categorys"
       }

   };
                var Gender = new List<FilterModelPropertyValues>()
   {
           new FilterModelPropertyValues()
       {
           Id=Guid.NewGuid().ToString(),
           Name="Male",Sender="Male",Forginkey="Gender"
       },
         new FilterModelPropertyValues()
       {
           Id=Guid.NewGuid().ToString(),
           Name="Famle",Sender="Famle",Forginkey="Gender"
       }
   };
                var Dialect = new List<FilterModelPropertyValues>()
   {
          new FilterModelPropertyValues()
       {
           Id=Guid.NewGuid().ToString(),
           Name="Najdi Accent",Sender="Najdi Accent",Forginkey="Dialect"
       },
         new FilterModelPropertyValues()
       {
           Id=Guid.NewGuid().ToString(),
           Name="Hejaz Accent",Sender="Hejaz Accent",Forginkey="Dialect"
       }
   };


                valueFilterModel = new ValueFilterModel()
                {
                    Type = Type,
                    Categary = Categorys,
                    Langague = Langague,
                    Gender = Gender,
                    Dialect = Dialect
                };
            }
            else
            {
                var Type = new List<FilterModelPropertyValues>()
{
    new FilterModelPropertyValues()
    {
        Id = Guid.NewGuid().ToString(),
        Name = "نص إلى نص", // Text2Text
        Sender = "T2T",
        Forginkey = "Categorys"
    },
    new FilterModelPropertyValues()
    {
        Id = Guid.NewGuid().ToString(),
        Name = "نص إلى كلام", // Text2Speech
        Sender = "T2S",
        Forginkey = "Categorys"
    },
    new FilterModelPropertyValues()
    {
        Id = Guid.NewGuid().ToString(),
        Name = "محادثة", // Chat
        Sender = "Chat",
        Forginkey = "Categorys"
    }
};

                var Categorys = new List<FilterModelPropertyValues>()
{
    new FilterModelPropertyValues()
    {
        Id = Guid.NewGuid().ToString(),
        Name = "أخبار", // News
        Sender = "News",
        Forginkey = "Categorys"
    },
    new FilterModelPropertyValues()
    {
        Id = Guid.NewGuid().ToString(),
        Name = "عام", // general
        Sender = "general",
        Forginkey = "Categorys"
    }
};

                var Langague = new List<FilterModelPropertyValues>()
{
    new FilterModelPropertyValues()
    {
        Id = Guid.NewGuid().ToString(),
        Name = "إنجليزي", // English
        Sender = "English",
        Forginkey = "Categorys"
    },
    new FilterModelPropertyValues()
    {
        Id = Guid.NewGuid().ToString(),
        Name = "عربي", // Arabic
        Sender = "Arabic",
        Forginkey = "Categorys"
    }
};

                var Gender = new List<FilterModelPropertyValues>()
{
    new FilterModelPropertyValues()
    {
        Id = Guid.NewGuid().ToString(),
        Name = "ذكر", // Male
        Sender = "Male",
        Forginkey = "Gender"
    },
    new FilterModelPropertyValues()
    {
        Id = Guid.NewGuid().ToString(),
        Name = "أنثى", // Female
        Sender = "Famle",
        Forginkey = "Gender"
    }
};

                var Dialect = new List<FilterModelPropertyValues>()
{
    new FilterModelPropertyValues()
    {
        Id = Guid.NewGuid().ToString(),
        Name = "لهجة نجدية", // Najdi Accent
        Sender = "Najdi Accent",
        Forginkey = "Dialect"
    },
    new FilterModelPropertyValues()
    {
        Id = Guid.NewGuid().ToString(),
        Name = "لهجة حجازية", // Hejaz Accent
        Sender = "Hejaz Accent",
        Forginkey = "Dialect"
    }
};



                valueFilterModel = new ValueFilterModel()
                {
                    Type = Type,
                    Categary = Categorys,
                    Langague = Langague,
                    Gender = Gender,
                    Dialect = Dialect
                };
            }


       
            return valueFilterModel;
        }
    }
    public class ValueFilterModel
    {


        public List<FilterModelPropertyValues> Type { get; set; }
        public List<FilterModelPropertyValues> Categary { get; set; }
        public List<FilterModelPropertyValues> Langague { get; set; }
        public List<FilterModelPropertyValues> Gender { get; set; }
        public List<FilterModelPropertyValues> Dialect { get; set; }
    }



    public class FilterModelPropertyValues
    {
        public string Id { get; set; }
        public string  Name { get; set; }
        public string Sender { get; set; }
        public string Forginkey { get; set; }



 

         



             

            

        

    }

}

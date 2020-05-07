using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.IO.Compression;
using System.Collections.Generic;

namespace new_work
{
    
    [Serializable]
    public class Dir
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public int Length { get; set; }
        public DateTime Date { get; set; }
        public Dir Dirr { get; set; }//вложенный

        public Dir(string a, string start)
        {
            try
            {
                Dir dir_mass=new Dir();
                foreach (string d in Directory.GetDirectories(a))
                {
                    F file_mass = new F();
                    foreach (string f in Directory.GetFiles(d))
                    {

                        //+файл
                        FileInfo fileInfo = new FileInfo(f);
                        file_mass= new F(fileInfo.Name.ToString(), f.ToString(), (int)fileInfo.Length, fileInfo.CreationTime);
                        Console.WriteLine("Объект создан");
                        XmlSerializer formatt = new XmlSerializer(typeof(F));
                        using (FileStream fs = new FileStream(start + "XML.xml", FileMode.OpenOrCreate))
                        {
                            formatt.Serialize(fs, file_mass);
                            Console.WriteLine("Объект сериализован");
                        }
                    }
                    //+папка
                    DirectoryInfo dirInfo = new DirectoryInfo(d);
                    if (Directory.GetDirectories(d).Length != 0)
                    {
                        Dir c = new Dir(d, start);
                        dir_mass = new Dir(dirInfo.Name.ToString(), d.ToString(), dirInfo.GetFiles().Length, dirInfo.CreationTime,c);
                        //при возврате вложенного объекта ничего не возвращается

                        XmlSerializer formatter = new XmlSerializer(typeof(Dir));
                        using (FileStream fs = new FileStream(start + "XML.xml", FileMode.OpenOrCreate))
                        {
                            formatter.Serialize(fs, dir_mass);
                            Console.WriteLine("Объект сериализован");
                        }
                    }
                    else
                    {

                        dir_mass = new Dir(dirInfo.Name.ToString(), d.ToString(), dirInfo.GetFiles().Length, dirInfo.CreationTime);
                    }
                    Console.WriteLine("Объект создан");
                }


            }
        catch (System.Exception excpt)
            {
                Console.WriteLine(excpt.Message);
            }
        }
        public Dir()
        { }
        public Dir(string name, string path, int length, DateTime date)
        {
            Name = name;
            Path = path;
            Length = length;
            Date = date;
        }
        public Dir(string name, string path, int length, DateTime date, Dir dir)
        {
            Name = name;
            Path = path;
            Length = length;
            Date = date;
            Dirr = dir;
        }
    }
    [Serializable]
    public class F
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public int Length { get; set; }
        public DateTime Date { get; set; }
        
        // стандартный конструктор без параметров
        public F()
        { }

        public F(string name, string path, int length, DateTime date)
        {
            Name = name;
            Path = path;
            Length = length;
            Date = date;
        }
    }
    class MainClass
    {
        private List<string> list = new List<string>();
        private string start_way;
        public void Init(string a)
        {
            start_way = a;
        }
        //new
        public static void Main(string[] args)
        {
            MainClass main = new MainClass();
            Dir dir = new Dir();
            if (args.Length == 0)
            {
                Dir d = new Dir(AppDomain.CurrentDomain.BaseDirectory, AppDomain.CurrentDomain.BaseDirectory);
            }
            else
            {
                Dir d = new Dir(args[0], args[0]);
            }
            System.IO.Directory.CreateDirectory(main.start_way + "ar");
            File.Copy(main.start_way + "XML.xml", main.start_way + "ar/XML.xml");
            if (!File.Exists(main.start_way + "ar.zip"))
            {
                ZipFile.CreateFromDirectory(@main.start_way + "ar", @main.start_way + "ar.zip");//в архив
            }
            //File.Delete(main.start_way + "XML.xml");//удаление промежутков
            File.Delete(main.start_way + "ar/XML.xml");
            System.IO.Directory.Delete(@main.start_way + "ar");
        }
    }
}

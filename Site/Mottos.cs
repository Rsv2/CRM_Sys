using Newtonsoft.Json;

namespace Site
{
    public static class Mottos
    {
        static List<Motto>? MottosList { get; set; }

        static Mottos() { }
        public static string GetMotto()
        {
            MottosList = JsonConvert.DeserializeObject<List<Motto>>(CRUD.Read("Mottos"));
            return MottosList[MyRandom.Next(0, MottosList.Count)].Text;
        }
    }
}

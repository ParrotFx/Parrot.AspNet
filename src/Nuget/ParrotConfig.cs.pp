[assembly: WebActivatorEx.PreApplicationStartMethod(typeof($rootnamespace$.App_Start.ParrotConfig), "Start")]
namespace $rootnamespace$.App_Start
{
    using Parrot.AspNet;

    public class ParrotConfig
    {
        public static void Start()
        {
            System.Web.Mvc.ViewEngines.Engines.Add(new ParrotViewEngine());
        }
    }
}
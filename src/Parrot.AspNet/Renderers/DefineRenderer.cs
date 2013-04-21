namespace Parrot.AspNet.Renderers
{
    using System.Collections.Generic;
    using System.Linq;
    using Parrot.Infrastructure;
    using Parrot.Nodes;
    using Parrot.Renderers;
    using Parrot.Renderers.Infrastructure;

    public class DefineRenderer : HtmlRenderer
    {

        public DefineRenderer(IHost host) : base(host) { }

        public override IEnumerable<string> Elements
        {
            get { yield return "define"; }
        }

        public override void Render(IParrotWriter writer, IRendererFactory rendererFactory, Statement statement, IDictionary<string, object> documentHost, object model)
        {
            //get the parameter
            List<string> parameters = new List<string>();
            if (statement.Parameters != null && statement.Parameters.Any())
            {
                //assume only the first is the path
                //second is the argument (model)
                //TODO: fix this

                //statement.Parameters[0].Value;

                parameters.AddRange(statement.Parameters.Select(p => p.CalculatedValue));
                documentHost.Add(parameters[0], parameters[1]);
            }
        }
    }
}
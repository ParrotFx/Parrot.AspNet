namespace Parrot.AspNet.Renderers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Web.Mvc;
    using System.Web.Mvc.Html;
    using System.Web.Routing;
    using Parrot.Infrastructure;
    using Parrot.Nodes;
    using Parrot.Renderers;
    using Parrot.Renderers.Infrastructure;

    public class ActionRenderer : HtmlRenderer
    {
        private MethodInfo[] _renderAction;

        public ActionRenderer(IHost host) : base(host)
        {
            _renderAction = this.GetType()
                .GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(method => method.Name == "RenderAction")
                .ToArray();
        }

        public override IEnumerable<string> Elements
        {
            get { yield return "action"; }
        }

        private void RenderAction(IParrotWriter writer, IDictionary<string, object> documentHost, string action)
        {
            RenderAction(writer, documentHost, action, null);
        }

        private void RenderAction(IParrotWriter writer, IDictionary<string, object> documentHost, string action, string controllerName)
        {
            RenderAction(writer, documentHost, action, controllerName, null);
        }

        private void RenderAction(IParrotWriter writer, IDictionary<string, object> documentHost, string action, string controllerName, string area, params string[] additional)
        {
            RouteValueDictionary routeValues = new RouteValueDictionary();
            routeValues["action"] = action;
            if (!string.IsNullOrWhiteSpace(controllerName))
            {
                routeValues["controller"] = controllerName;
            }
            if (!string.IsNullOrWhiteSpace(area))
            {
                routeValues["area"] = area;
            }

            var viewContext = documentHost["ViewContext"] as ViewContext;
            IViewDataContainer container = new ViewPage();
            var helper = new HtmlHelper(viewContext, container);
            writer.Write(helper.Action(action, routeValues).ToHtmlString());
        }

        public override void Render(IParrotWriter writer, IRendererFactory rendererFactory, Statement statement, IDictionary<string, object> documentHost, object model)
        {
            //get the parameter
            List<object> parameters = new List<object>()
                {
                    writer, 
                    documentHost
                };

            if (statement.Parameters != null && statement.Parameters.Any())
            {
                //assume only the first is the path
                //second is the argument (model)
                //TODO: fix this
                parameters.AddRange((List<object>)GetLocalModel(documentHost, statement, model));
            }
            else
            {
                //layout = "_layout";
                throw new InvalidOperationException("Missing parameters");
            }

            var call = _renderAction.SingleOrDefault(r => r.GetParameters().Count() == parameters.Count);

            if (call != null)
            {
                call.Invoke(this, parameters.ToArray());
            }
            else
            {
                //throw an exception
            }
        }
    }
}
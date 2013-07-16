namespace Parrot.AspNet.Renderers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Parrot.AspNet;
    using Parrot.Infrastructure;
    using Parrot.Nodes;
    using Parrot.Renderers;
    using Parrot.Renderers.Infrastructure;

    public class PartialRenderer : HtmlRenderer
    {
        public PartialRenderer(IHost host)
            : base(host)
        {
        }

        public override IEnumerable<string> Elements
        {
            get { yield return "partial"; }
        }

        public override void Render(IParrotWriter writer, IRendererFactory rendererFactory, Statement statement, IDictionary<string, object> documentHost, object model)
        {
            //get the parameter
            var partial = GetLocalModel(documentHost, statement, model) as string;
            if (string.IsNullOrEmpty(partial))
            {
                throw new ArgumentNullException("partial");
            }
            //ok...we need to load the layoutpage
            //then pass the node's children into the layout page
            //then return the result
            var engine = (Host as AspNetHost).ViewEngine;
            var result = engine.FindView(null, partial, null, false);
            if (result != null)
            {
                var parrotView = (result.View as ParrotView);
                using (var stream = parrotView.LoadStream())
                {
                    var document = parrotView.LoadDocument(stream);

                    DocumentView view = new DocumentView(Host, rendererFactory, documentHost, document);

                    view.Render(writer);
                }
            }
        }
    }
}

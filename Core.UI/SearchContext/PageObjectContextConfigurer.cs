using Core.UI.SearchContext.Abstractions;

namespace Core.UI.SearchContext
{
    public interface IContextEventHandlers
    {
        protected Type ContextType { get; }
        internal void OnContextOpened(object s, EventArgs e);
        internal void OnContextClosed(object s, EventArgs e);
        internal void OnBeforeAction(object s, ActionEventArgs e);
        internal void OnAfterAction(object s, ActionEventArgs e);
        public void SubscribeHandlers(UiContext context)
        {
            var contextType = context.GetType();

            if (contextType == ContextType || contextType.IsSubclassOf(ContextType))
            {
                //context.OnContextOpened += OnContextOpened;
                //context.OnContextClosed += OnContextClosed;
                //context.OnBeforeAction += OnBeforeAction;
                //context.OnAfterAction += OnAfterAction;
            }
        }
    }

    public abstract class ContextEventHandler<TContext> : IContextEventHandlers where TContext : UiContext
    {
        Type IContextEventHandlers.ContextType => typeof(TContext);
        void IContextEventHandlers.OnContextOpened(object s, EventArgs e) => OnContextOpened((TContext)s, e);
        void IContextEventHandlers.OnContextClosed(object s, EventArgs e) => OnContextClosed((TContext)s, e);
        void IContextEventHandlers.OnBeforeAction(object s, ActionEventArgs e) => OnBeforeAction((TContext)s, e);
        void IContextEventHandlers.OnAfterAction(object s, ActionEventArgs e) => OnAfterAction((TContext)s, e);

        public virtual void OnContextOpened(TContext s, EventArgs e) { }
        public virtual void OnContextClosed(TContext s, EventArgs e) { }
        public virtual void OnBeforeAction(TContext s, ActionEventArgs e) { }
        public virtual void OnAfterAction(TContext s, ActionEventArgs e) { }

    }

    //public class PageObjectContextConfigurer
    //{
    //    private Dictionary<Type, PageObjectContextConfigurer> _handlersByType = new();

    //    readonly List<UiContext.ContextEventHandler<ContextEventArgs>> _contextOpenedHandlers = new();
    //    readonly List<UiContext.ContextEventHandler<ContextEventArgs>> _contextClosedHandlers = new();
    //    readonly List<ActionEventHandler<ActionEventArgs>> _beforeActionHandlers = new();
    //    readonly List<ActionEventHandler<ActionEventArgs>> _afterActionHandlers = new();

    //    public static void RegisterHandlers<TContext>(ContextEventHandler<TContext> handler) where TContext : UiContext
    //    {
    //        var typeHandlers = GetTypeHandlers(handler.ContextType);

    //        typeHandlers._contextOpenedHandlers.Add(handler.OnContextOpened);
    //        typeHandlers._contextClosedHandlers.Add(handler.OnContextClosed);
    //        typeHandlers._beforeActionHandlers.Add(handler.OnBeforeAction);
    //        typeHandlers._afterActionHandlers.Add(handler.OnAfterAction);
    //    }

    //    internal static void AssignHandlers(UiContext component)
    //    {
    //        var componentType = component.GetType();
    //        var typeHandlers = GetTypeHandlers(componentType);

    //        foreach (var handler in typeHandlers._contextOpenedHandlers)
    //        {
    //            component.OnContextOpened += handler;
    //        }
    //        foreach (var handler in typeHandlers._contextClosedHandlers)
    //        {
    //            component.OnContextClosed += handler;
    //        }
    //        foreach (var handler in typeHandlers._beforeActionHandlers)
    //        {
    //            component.OnBeforeAction += handler;
    //        }
    //        foreach (var handler in typeHandlers._afterActionHandlers)
    //        {
    //            component.OnAfterAction += handler;
    //        }
    //    }

    //    private PageObjectContextConfigurer GetTypeHandlers(Type contextType)
    //    {
    //        if (_handlersByType.TryGetValue(contextType, out var handlers))
    //        {
    //            return handlers; 
    //        }
    //        else
    //        {
    //            var newConfigs = new PageObjectContextConfigurer();
    //            _handlersByType[contextType] = newConfigs;
    //            return newConfigs;
    //        }
    //    }
    ////}
}

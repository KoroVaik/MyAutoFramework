using Core.UI.SearchContext.Component;

namespace Core.UI.SearchContext.Abstractions
{
    //public abstract class UiContext<TContext> : UiContext where TContext : UiContext<TContext>
    //{
    //}

    public abstract class UiContext
    {
        internal protected UiContext? ParentContext { get; set; }
        protected List<BaseComponent> ChildComponents { get; } = new();
        protected ContextEventHandlers ContextEventHandlers { get; private set; } = null!;
        internal void SetEventHandlers(ContextEventHandlers eventHandlers)
        {
            ContextEventHandlers = eventHandlers;
        }

        internal void OpenContext()
        {
            ContextEventHandlers.OnContextOpened(this, new ContextEventArgs());

        }

        internal void CloseContext()
        {
            ChildComponents.ForEach(c => c.CloseContext());
            ContextEventHandlers.OnContextClosed(this, new ContextEventArgs());
        }
    }

    public class CustomContextEventHandlers : Dictionary<Type, ContextEventHandlers>
    {
        internal void AssignHandlers(UiContext context)
        {
            var instanceContextType = context.GetType();

            foreach (var handlers in this)
            {
                var handlerTypeKey = handlers.Key;
                if (instanceContextType == handlerTypeKey || instanceContextType.IsSubclassOf(handlerTypeKey))
                {
                    context.SetEventHandlers(handlers.Value);
                }
            }
        }
    }

    public abstract class ContextEventHandlers
    {
        public virtual void OnContextOpened(UiContext context, ContextEventArgs args)
        { }
        public virtual void OnContextClosed(UiContext context, ContextEventArgs args)
        { }
        public virtual void OnBeforeAction(UiContext context, ActionEventArgs args)
        { }
        public virtual void OnAfterAction(UiContext context, ActionEventArgs args)
        { }

        //public delegate void ContextEventHandler<TArgs>(UiContext sender, TArgs args) where TArgs : ContextEventArgs;
        //public delegate void ActionEventHandler<TArgs>(UiContext sender, TArgs args) where TArgs : ActionEventArgs;

        //internal event ContextEventHandler<ContextEventArgs>? OnContextOpened;
        //internal event ContextEventHandler<ContextEventArgs>? OnContextClosed;
        //internal event ActionEventHandler<ActionEventArgs>? OnBeforeAction;
        //internal event ActionEventHandler<ActionEventArgs>? OnAfterAction;

    }
}

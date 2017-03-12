using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
using System.Windows.Media;

namespace OneComic.Admin.Behaviors
{
    public sealed class TreeViewSelectedItemBindingBehavior : Behavior<TreeView>
    {
        public object SelectedItem
        {
            get { return GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register(
                "SelectedItem", 
                typeof(object), 
                typeof(TreeViewSelectedItemBindingBehavior),
                new FrameworkPropertyMetadata(null, OnSelectedItemChanged) { BindsTwoWayByDefault = true });

        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.SelectedItemChanged += OnTreeViewSelectedItemChanged;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            if (AssociatedObject != null)
                AssociatedObject.SelectedItemChanged -= OnTreeViewSelectedItemChanged;
        }

        private void OnTreeViewSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            SelectedItem = e.NewValue;
        }

        private static void OnSelectedItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var behavior = d as TreeViewSelectedItemBindingBehavior;
            var tvi = GetTreeViewItem(behavior.AssociatedObject, e.NewValue);
            if (tvi != null)
                tvi.SetValue(TreeViewItem.IsSelectedProperty, true);
        }

        /* Refer to http://msdn.microsoft.com/en-us/library/ff407130(v=vs.110).aspx 
         * for implementation details of this method */
        private static TreeViewItem GetTreeViewItem(ItemsControl container, object item)
        {
            if (container == null)
                return null;

            if (container.DataContext == item)
                return container as TreeViewItem;

            // Expand the current container
            if (container is TreeViewItem && !((TreeViewItem)container).IsExpanded)
                container.SetValue(TreeViewItem.IsExpandedProperty, true);

            // Try to generate the ItemsPresenter and the ItemsPanel.
            // by calling ApplyTemplate.  Note that in the 
            // virtualizing case even if the item is marked 
            // expanded we still need to do this step in order to 
            // regenerate the visuals because they may have been virtualized away.

            container.ApplyTemplate();
            var itemsPresenter = (ItemsPresenter)container.Template.FindName("ItemsHost", container);
            if (itemsPresenter != null)
            {
                itemsPresenter.ApplyTemplate();
            }
            else
            {
                // The Tree template has not named the ItemsPresenter, 
                // so walk the descendents and find the child.
                itemsPresenter = FindVisualChild<ItemsPresenter>(container);
                if (itemsPresenter == null)
                {
                    container.UpdateLayout();

                    itemsPresenter = FindVisualChild<ItemsPresenter>(container);
                }
            }

            var itemsHostPanel = (Panel)VisualTreeHelper.GetChild(itemsPresenter, 0);

            // Ensure that the generator for this panel has been created.
            var children = itemsHostPanel.Children;

            var virtualizingPanel = itemsHostPanel as VirtualizingStackPanel;

            for (int i = 0, count = container.Items.Count; i < count; i++)
            {
                TreeViewItem subContainer;
                if (virtualizingPanel != null)
                {
                    // Bring the item into view so 
                    // that the container will be generated.
                    virtualizingPanel.BringIndexIntoViewPublic(i);

                    subContainer = (TreeViewItem)container.ItemContainerGenerator.ContainerFromIndex(i);
                }
                else
                {
                    subContainer = (TreeViewItem)container.ItemContainerGenerator.ContainerFromIndex(i);

                    // Bring the item into view to maintain the 
                    // same behavior as with a virtualizing panel.
                    subContainer.BringIntoView();
                }

                if (subContainer == null)
                    continue;
                
                // Search the next level for the object.
                TreeViewItem resultContainer = GetTreeViewItem(subContainer, item);
                if (resultContainer != null)
                    return resultContainer;

                // The object is not under this TreeViewItem
                // so collapse it.
                subContainer.IsExpanded = false;
            }

            return null;
        }

        /* Refer to http://msdn.microsoft.com/en-us/library/ff407130(v=vs.110).aspx 
         * for implementation details of this method */
        private static T FindVisualChild<T>(Visual visual) where T : Visual
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(visual); i++)
            {
                var child = (Visual)VisualTreeHelper.GetChild(visual, i);
                if (child == null)
                    continue;

                T correctlyTyped = child as T;
                if (correctlyTyped != null)
                    return correctlyTyped;

                T descendent = FindVisualChild<T>(child);
                if (descendent != null)
                    return descendent;
            }
            return null;
        }
    }
}

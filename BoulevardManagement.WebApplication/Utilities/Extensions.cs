using Kendo.Mvc.UI;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using BoulevardManagement.WebApplication.Models;
using BoulevardManagement.DTO;
using BoulevardManagement.Model.Common;
using System;

namespace BoulevardManagement.WebApplication
{
    public static class Extensions
    {
        public static bool IsInRoles(this IPrincipal user, params string[] roles)
        {
            return roles.Any(d => user.IsInRole(d));
        }

        public static List<TreeViewItemModel> BuildTree(this List<ApplicationRole> roles, List<ApplicationRole> selectedRoles = null, bool isGroomingEnabled = true)
        {
            List<TreeViewItemModel> result = new List<TreeViewItemModel>();

            foreach (var role in roles.OrderBy(c => c.Name))
            {
                var nodes = !string.IsNullOrEmpty(role.Description) ? role.Description.Split('\\') : role.Name.Split('\\');
                var currentLevel = result.SingleOrDefault(c => c.Text == nodes.First());

                if (currentLevel == null)
                {
                    currentLevel = new TreeViewItemModel() { Text = nodes.First(), Expanded = false };
                    result.Add(currentLevel);
                }

                foreach (var node in nodes.Skip(1))
                {
                    var treeItem = getItemByText(currentLevel, node);
                    if (treeItem == null)
                    {
                        treeItem = new TreeViewItemModel() { Text = node, Id = string.Empty };
                        currentLevel.Items.Add(treeItem);
                    }
                    currentLevel = treeItem;
                }

                currentLevel.Id = role.Id;
                if (selectedRoles != null)
                    currentLevel.Checked = selectedRoles.Any(d => d.Id == role.Id);
            }

            if (isGroomingEnabled)
                return grooming(result);
            else
                return result;
        }



        public static List<TreeViewItemModel> grooming(List<TreeViewItemModel> tree)
        {
            List<TreeViewItemModel> result = new List<TreeViewItemModel>();
            foreach (var group in RoleConsistent.Groups)
            {
                TreeViewItemModel node = new TreeViewItemModel();
                node.Expanded = true;
                node.Text = group.Key;
                node.Id = group.Key;
                //node.HtmlAttributes.Add("class", "box-title");

                foreach (var groupItem in group.Value)
                    node.Items.Add(tree.Where(c => c.Text == groupItem).FirstOrDefault());

                result.Add(node);
            }

            return result;
        }

        public static List<TreeViewItemModel> BuildTree(this List<NotificationDTO> notifications, List<NotificationDTO> selectedNotifications = null, bool isGroomingEnabled = true)
        {
            List<TreeViewItemModel> result = new List<TreeViewItemModel>();

            foreach (var notification in notifications.OrderBy(c => c.Name))
            {
                var nodes = notification.Name.Split('\\');
                var currentLevel = result.SingleOrDefault(c => c.Text == nodes.First());

                if (currentLevel == null)
                {
                    currentLevel = new TreeViewItemModel() { Text = nodes.First(), Expanded = false };
                    result.Add(currentLevel);
                }

                foreach (var node in nodes.Skip(1))
                {
                    var treeItem = getItemByText(currentLevel, node);
                    if (treeItem == null)
                    {
                        treeItem = new TreeViewItemModel() { Text = node, Id = string.Empty };
                        currentLevel.Items.Add(treeItem);
                    }
                    currentLevel = treeItem;
                }

                currentLevel.Id = notification.Id.ToString();
                if (selectedNotifications != null)
                    currentLevel.Checked = selectedNotifications.Any(d => d.Id == notification.Id);
            }

            if (isGroomingEnabled)
                return NotificationGrooming(result);
            else
                return result;
            //return result;
        }

        private static List<TreeViewItemModel> NotificationGrooming(List<TreeViewItemModel> tree)
        {
            List<TreeViewItemModel> result = new List<TreeViewItemModel>();
            foreach (var group in NotificationConsistent.Groups)
            {
                TreeViewItemModel node = new TreeViewItemModel();
                node.Expanded = true;
                node.Text = group.Key;
                node.Id = group.Key;
                //node.HtmlAttributes.Add("class", "box-title");

                foreach (var groupItem in group.Value)

                    node.Items.Add(tree.Where(c => c.Text == groupItem).FirstOrDefault());

                result.Add(node);
            }




            return result;
        }




      

        private static TreeViewItemModel getItemByText(TreeViewItemModel root, string text)
        {
            foreach (var item in root.Items)
                if (item.Text == text)
                    return item;
                else
                {
                    var subItem = getItemByText(item, text);
                    if (subItem != null)
                        return subItem;
                }

            return null;
        }


        public static IEnumerable<TSource> DistinctBy<TSource, TKey>
    (this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {

            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }

    
    }
}
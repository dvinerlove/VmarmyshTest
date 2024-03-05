using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Reflection.Emit;
using System.Text.Json.Serialization;
using System.Xml.Linq;

namespace VmarmyshTest.Models
{
    public static class NodeExtensions
    {
        public static bool IsBelongsTo(this Node child, Node tree)
        {
            return tree.Descendants().Any(node => node.Id == child.Id);
        }

        public static bool HasName(this Node tree, string name)
        {
            return tree.Descendants().Any(node => node.Name == name);
        }

        public static Node LoadEntireTree(this AppDbContext context, string treeName)
        {
            var tree = context.Nodes.ToArray()
                .FirstOrDefault(x => x.Name == treeName && x.ParentId == null);

            return tree ?? new Node() { Name = treeName };
        }

        public static IEnumerable<Node> Descendants(this Node root)
        {
            var nodes = new Stack<Node>(new[] { root });
            while (nodes.Any())
            {
                Node node = nodes.Pop();
                yield return node;
                foreach (var n in node.Children) nodes.Push(n);
            }
        }
    }

    public class Node
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [ScaffoldColumn(true)]
        public string Name { get; set; }
        [Required]
        public virtual ICollection<Node> Children { get; set; } = new List<Node>();
        [JsonIgnore]
        [AllowNull]
        public int? ParentId { get; set; } 

        [JsonIgnore]
        [AllowNull]
        public Node? Parent { get; set; } 
    }
}

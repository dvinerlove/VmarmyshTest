using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using VmarmyshTest.Models;

namespace VmarmyshTest.Controllers
{
    [ApiController]
    [Tags("User/Tree/Node")]
    [Route("Api/User/Tree/Node")]
    public class TreeNodeController : AppControllerBase
    {
        private readonly ILogger<TreeNodeController> _logger;
        private readonly AppDbContext _context;

        public TreeNodeController(ILogger<TreeNodeController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpPost("Rename")]
        public IActionResult RenameNode([BindRequired] string? treeName,
                                        [BindRequired] string? nodeId,
                                        [BindRequired] string? newNodeName)
        {
            this.CheckRequiredFields(MethodBase.GetCurrentMethod());
            Node? requestedNode = GetNodeById(nodeId);
            Node? parent = GetNodeById(requestedNode.ParentId.Value);
            GetTree(treeName, requestedNode);

            if (parent.HasName(newNodeName!.Trim()))
            {
                throw new SecureException("Dublicate name");
            }

            requestedNode.Name = newNodeName!.Trim();
            _context.Update(requestedNode);
            _context.SaveChanges();

            return Ok();
        }

        [HttpPost("Create")]
        public IActionResult CreateNode([BindRequired] string? treeName,
                                        [BindRequired] string? parentNodeId,
                                        [BindRequired] string? nodeName)
        {

            this.CheckRequiredFields(MethodBase.GetCurrentMethod());
            Node? requestedNode = GetNodeById(parentNodeId);
            GetTree(treeName, requestedNode);

            if (requestedNode.HasName(nodeName!.Trim()))
            {
                throw new SecureException("Dublicate name");
            }

            requestedNode.Children.Add(new Node() { Name = nodeName!.Trim() });
            _context.Update(requestedNode);
            _context.SaveChanges();

            return Ok();
        }

        [HttpPost("Delete")]
        public IActionResult DeleteNode([BindRequired] string? treeName,
                                        [BindRequired] string? nodeId)
        {
            this.CheckRequiredFields(MethodBase.GetCurrentMethod());
            Node? requestedNode = GetNodeById(nodeId);
            GetTree(treeName, requestedNode);

            if (requestedNode.Children.Any())
            {
                throw new SecureException("You have to delete all children nodes first");
            }

            _context.Remove(requestedNode);
            _context.SaveChanges();
            return Ok();
        }

        private Node GetTree(string treeName, Node requestedNode)
        {
            Node tree = _context.LoadEntireTree(treeName!.Trim());
            if (requestedNode.IsBelongsTo(tree) == false)
            {
                throw new SecureException($"Requested node was found, but it doesn't belong your tree");
            }
            return tree;
        }

        private Node GetNodeById(string parentNodeId)
        {
            return GetNodeById(int.Parse(parentNodeId.Trim()));
        }

        private Node GetNodeById(int parentNodeId)
        {
            var requestedNode = _context.Nodes.Include(x => x.Children).FirstOrDefault(node => node.Id == parentNodeId);
            if (requestedNode == null)
            {
                throw new SecureException($"Node with ID = {parentNodeId} was not found");
            }

            return requestedNode;
        }
    }
}
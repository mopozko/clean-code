using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public class AhoCorasickTree
    {
        private readonly AhoCorasickNode root = new AhoCorasickNode();

        public AhoCorasickTree(IEnumerable<string> values)
        {
            root = new AhoCorasickNode();
            foreach (var value in values)
                Add(value);
            Determine();
        }

        private void Add(string value)
        {
            var node = root;
            foreach (var c in value)
                node = node[c] ?? (node[c] = new AhoCorasickNode(c, node));
            node.Word = value;
        }
        private void Determine()
        {
            var queue = new Queue<AhoCorasickNode>();
            queue.Enqueue(root);

            while (queue.Count > 0)
            {
                var node = queue.Dequeue();
                foreach (var child in node.GetChildren)
                    queue.Enqueue(child);
                if (node == root)
                {
                    root.Fail = root;
                    continue;
                }
                var fail = node.Parent.Fail;
                while (fail[node.Value] == null && fail != root)
                    fail = fail.Fail;
                node.Fail = fail[node.Value] ?? root;
                if (node.Fail == node)
                    node.Fail = root;
            }
        }

        public IEnumerable<SingleMarkupToken> Find(string text)
        {
            var node = root;

            for (var index = 0; index < text.Length; index++)
            {
                var c = text[index];
                while (node[c] == null && node != root)
                    node = node.Fail;
                node = node[c] ?? root;
                if (node.IsFinish)
                    yield return new SingleMarkupToken(node.Word, index + 1 - node.Word.Length);
            }
        }

        private class AhoCorasickNode
        {
            private Dictionary<char, AhoCorasickNode> Children { get; }

            public char Value { get; }
            public string Word { get; set; }
            public AhoCorasickNode Parent { get; }
            public AhoCorasickNode Fail { get; set; }

            public bool IsFinish => Word != null;
            public AhoCorasickNode()
            {
                Children = new Dictionary<char, AhoCorasickNode>();
            }

            public AhoCorasickNode(char value, AhoCorasickNode parent) : this()
            {
                this.Value = value;
                this.Parent = parent;
            }

            public IEnumerable<AhoCorasickNode> GetChildren => Children.Values;
            public AhoCorasickNode this[char c]
            {
                get => Children.ContainsKey(c) ? Children[c] : null;
                set => Children[c] = value;
            }
            public override string ToString()
            {
                return Value.ToString();
            }
        }
    }
}
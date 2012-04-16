using Contracts;
class IntLinkedList {
    private Node Head;

    public void PushFront(Node node) {
        Contract.Memory.Tmp<Logger>(1);
        Contract.Memory.DestTmp();
        Logger logger = new Logger();
        node.Next = this.Head;
        this.Head = node;
        logger.Log("PushFront done");
    }

    public void Fill(int n) {
        System.Diagnostics.Contracts.Contract.Requires(n > 0);
        Contract.Memory.Rsd<Node>(Contract.Memory.This, n - 1);
        Contract.Memory.Tmp<Logger>(1);
        for (int i = 1; i <= n; i++) {
            Contract.Memory.DestRsd(Contract.Memory.This);
            Node node = new Node(i);
            this.PushFront(node);
        }
    }

    public void Clear() {
        this.Head = null;
        Contract.Memory.Rsd<Logger>(Contract.Memory.This, 1);
        Contract.Memory.DestRsd(Contract.Memory.This);
        Logger logger = new Logger();
        logger.Log("Clear done");
    }
}

class Node
{
    public Node Next { get; set; }
    public Node(int value)
    {
    }
}

class Logger
{
    public void Log(string message)
    {
    }
}
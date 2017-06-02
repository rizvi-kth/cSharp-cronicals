namespace ControllerAndWorkspacesExample.Workspaces
{
    public interface IWorkspaceDescriptor
    {
        int Position { get; }

        string Name { get; }

        Workspace CreateWorkspace();
    }
}
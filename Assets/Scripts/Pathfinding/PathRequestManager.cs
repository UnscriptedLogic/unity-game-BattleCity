using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathRequestManager : Semaphore
{
    private Queue<PathRequest> pathRequestsQueue = new Queue<PathRequest>();
    private PathRequest currentPathRequest;
    private PathFinder pathFinder;

    private bool isProcessingPath;

    #region Singleton
    public static PathRequestManager instance;
    protected override void SephamoreStart(Manager manager)
    {
        base.SephamoreStart(manager);
        instance = this;
        pathFinder = GetComponent<PathFinder>();
    }
    #endregion

    public static void RequestPath(Vector3 pathStart, Vector3 pathEnd, Action<Vector3[], bool> callback)
    {
        PathRequest newRequest = new PathRequest(pathStart, pathEnd, callback);
        instance.pathRequestsQueue.Enqueue(newRequest);
        instance.TryProcessNext();
    }

    private void TryProcessNext()
    {
        if (!isProcessingPath && pathRequestsQueue.Count > 0)
        {
            currentPathRequest = pathRequestsQueue.Dequeue();
            isProcessingPath = true;
            pathFinder.StartFindPath(currentPathRequest.pathStart, currentPathRequest.pathEnd);
        }
    }

    public void FinishProcessingPath(Vector3[] path, bool success)
    {
        currentPathRequest.callback(path, success);
        isProcessingPath = false;
        TryProcessNext();
    }

    private struct PathRequest
    {
        public Vector3 pathStart;
        public Vector3 pathEnd;
        public Action<Vector3[], bool> callback;

        public PathRequest(Vector3 pathStart, Vector3 pathEnd, Action<Vector3[], bool> callback)
        {
            this.pathStart = pathStart;
            this.pathEnd = pathEnd;
            this.callback = callback;
        }
    }
}
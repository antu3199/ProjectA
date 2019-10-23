using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public sealed class TimeManager : MonoBehaviour, IGameManager
{
    // 60fps's FixedDeltaTime
    private const float FIXED_DELTA_TIME_BASE = (1f / 60f);
    private float m_deltaTime;
    private float m_deltaTimeUnscaled;
    private float m_deltaTimeFixed;

    private float m_deltaFrameCount;
    private float m_deltaFrameCountUnscaled;
    private float m_deltaFrameCountFixed;

    private float m_totalTime;  
    private float m_totalFrameCount;
    private float m_totalFrameCountUnscaled;
    private float m_totalFrameCountFixed;

    private bool m_pausing;

    public bool pausing { get { return m_pausing; } }

    public struct ScheduledTask {
      public float StartTime;
      public float EndTime;
      public float Duration;
      public UnityAction OnComplete;
      public UnityAction<float> OnUpdate;

      public ScheduledTask(float time, UnityAction onComplete, UnityAction<float> OnUpdate = null) {
        this.StartTime = 0;
        this.EndTime = 0;
        this.Duration = time;
        this.OnComplete = onComplete;
        this.OnUpdate = OnUpdate;
      }
    }

    private List<ScheduledTask> scheduleTasks;

    public void Initialize() {
      this.scheduleTasks = new List<ScheduledTask>();
    }

    private void Update()
    {
        if (!pausing) {
          UpdateTimes();
        }
    }

    private void UpdateTimes()
    {
        m_deltaTime = Time.deltaTime;
        m_totalTime += m_deltaTime;
        m_deltaTimeUnscaled = Time.unscaledDeltaTime;

        float nowFps = 0;
        int vSyncCount = QualitySettings.vSyncCount;
        if (vSyncCount == 1)
        {
            nowFps = Screen.currentResolution.refreshRate;
        }
        else if (vSyncCount == 2)
        {
            nowFps = Screen.currentResolution.refreshRate / 2f;
        }
        else
        {
            nowFps = Application.targetFrameRate;
        }

        if (nowFps > 0)
        {
            m_deltaTimeFixed = FIXED_DELTA_TIME_BASE * (60 / nowFps);
        }
        else
        {
            m_deltaTimeFixed = 0;
        }

        m_deltaFrameCount = m_deltaTime / FIXED_DELTA_TIME_BASE;
        m_deltaFrameCountUnscaled = m_deltaTimeUnscaled / FIXED_DELTA_TIME_BASE;
        m_deltaFrameCountFixed = m_deltaTimeFixed / FIXED_DELTA_TIME_BASE;

        m_totalFrameCount += m_deltaFrameCount;
        m_totalFrameCountUnscaled += m_deltaFrameCountUnscaled;
        m_totalFrameCountFixed += m_deltaFrameCountFixed;
        
        this.CheckScheduledTasks();
    }

    public ScheduledTask ScheduleTask(float time, UnityAction action, UnityAction<float> onUpdate = null) {
      ScheduledTask task = new ScheduledTask(time, action, onUpdate);
      task.StartTime = this.m_totalTime;
      task.EndTime = this.m_totalTime + task.Duration;
      this.scheduleTasks.Add(task);
      return task;
    }

    public void RemoveTask (ScheduledTask task) {
      this.scheduleTasks.Remove(task);
    }

    private void CheckScheduledTasks() {
      for (int i = this.scheduleTasks.Count - 1; i >= 0; i--) {
        if (this.m_totalTime >= this.scheduleTasks[i].EndTime) {
          if (this.scheduleTasks[i].OnComplete != null) {
            this.scheduleTasks[i].OnComplete();
          }
          this.scheduleTasks.Remove(this.scheduleTasks[i]);
        } else {
          float a = (this.m_totalTime - this.scheduleTasks[i].StartTime) / this.scheduleTasks[i].Duration;
          if (this.scheduleTasks[i].OnUpdate != null) {
            this.scheduleTasks[i].OnUpdate(a);
          }
        }
      }
    }

    public void Pause()
    {
        m_pausing = true;
    }

    public void Resume()
    {
        m_pausing = false;
    }
}
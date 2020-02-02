﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState {
    void OnEnter(IStateMachine controller);
    void Run(IStateMachine controller);
    void OnExit(IStateMachine controller);
}

public interface IStateMachine {
    IState CurrentState { get; set; }
}

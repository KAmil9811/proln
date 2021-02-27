import React, { Component } from 'react';
import { MachineTable } from './MachineTable';
import './MachineWarehouse.css'
import Sidebar from '../Sidebar';


export class MachineWarehouse extends Component {
    displayName = MachineWarehouse.name;
    constructor(props) {
        super(props);
    }
    logOut = (event) => {
        event.preventDefault();
        sessionStorage.removeItem('email')
        sessionStorage.removeItem('login')
        sessionStorage.removeItem('name')
        sessionStorage.removeItem('password')
        sessionStorage.removeItem('surname')
        sessionStorage.removeItem('user')
        sessionStorage.removeItem('admin')
        sessionStorage.removeItem('superAdmin')
        sessionStorage.removeItem('manager')
        sessionStorage.removeItem('magazineManagement')
        sessionStorage.removeItem('machineManagement')
        sessionStorage.removeItem('orderManagement')
        sessionStorage.removeItem('cutManagement')
        this.props.history.push('/')
    }


    homePage = (event) => {
        this.props.history.push('/home')
    }

    addMachine = (event) => {
        this.props.history.push('add_cutmachine')
    }

    historyMachine = (event) => {
        this.props.history.push('all_machine_history')
    }

    goback = (event) => {
        this.props.history.push('/')
    }

    goback2 = (event) => {
        this.props.history.push('/home')
    }
  /*  history() {
        if (sessionStorage.getItem('admin') === 'true' || sessionStorage.getItem('superAdmin') === 'true' || sessionStorage.getItem('manager') === 'true') {
            return (
   //             <button type="button" className="history" onClick={this.historyMachine}> Historia</button>
            )
        }
    }*/

    render() {
        //   let history = this.history();
        if (sessionStorage.getItem('valid') === '') {
            return (
                <div className="HomePage">
                    <h1>Log in to have access!</h1>
                    <button type="submit" className="success_login2" onClick={this.goback} >Log in</button>
                </div>
            );
        }
        else if (sessionStorage.getItem('machineManagement') === 'true' || sessionStorage.getItem('superAdmin') === 'true' || sessionStorage.getItem('manager') === 'true' || sessionStorage.getItem('admin') === 'true') {
            return (
                <div>

                    <Sidebar />
                    <div className="title">
                        <h1 className="titletext"> Machines</h1>
                    </div>

                    <div className="machine_warhouse_conteiner">
                        <button className="success_cm_add_wr" onClick={this.addMachine}>Add machine</button>
                        <MachineTable />
                    </div>
                </div>
            );
        }
        else {
            return (
                <div>

                    <Sidebar />
                    <div className="title">
                        <h1 className="titletext"> Machines</h1>
                    </div>

                    <div className="machine_warhouse_conteiner">
                        <MachineTable />
                    </div>
                </div>
            );

        }

    }
}
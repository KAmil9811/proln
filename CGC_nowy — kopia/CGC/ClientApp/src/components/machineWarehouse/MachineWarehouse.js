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

    history() {
        if (sessionStorage.getItem('admin') === 'true' || sessionStorage.getItem('superAdmin') === 'true' || sessionStorage.getItem('manager') === 'true') {
            return (
                <button type="button" className="history" onClick={this.historyMachine}> Historia</button>
            )
        }
    }

    render() {
        let history = this.history();
        return (
            <div>
<<<<<<< HEAD

                <Sidebar />
=======
                <div className="nav_mw">
                    <button type="button" className="log_out2" onClick={this.logOut}>Wyloguj</button>
                    {history}
                    <button type="button" className="home2" onClick={this.homePage}>Strona główna</button>
              

                </div>
>>>>>>> c6d0186b2c0a7675e26792e193a9f957855f8e9c
                <div className="conteiner_mw">
                    <button className="add_machine" onClick={this.addMachine}>Dodaj maszynę</button>
                    <MachineTable />
                </div>


                
            </div>
        );
    }
}
import React, { Component } from 'react';
import { AllMachineHistoryTable } from './AllMachineHistoryTable';


export class AllMachineHistory extends Component {
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

    render() {
        return (
            <div>
                <div className="nav_mw">
                    <button type="button" className="log_out2" onClick={this.logOut}>Wyloguj</button>
                    <button type="button" className="home2" onClick={this.homePage}>Strona główna</button>
                </div>
                <div className="conteiner_mw">
                    <AllMachineHistoryTable />
                </div>
            </div>
        );
    }
}
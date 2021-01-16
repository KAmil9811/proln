import React, { Component } from 'react';
import { AllMachineHistoryTable } from './AllMachineHistoryTable';
import Sidebar from '../Sidebar';


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
            <div className="AllMachinehistory">
                    <Sidebar />
                    <div className="allMachineHistory">
                
                        <div>
                            <div className="nav_mw">
                        
                    
                            </div>
                            <div className="conteiner_mw">
                            <AllMachineHistoryTable />
                            </div>
                        </div>
                    </div>
            </div>
        );
    }
}
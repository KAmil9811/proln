import React, { Component } from 'react';
import { UserHistoryTable } from './UserHistoryTable';
import Sidebar from '../Sidebar';


export class UserHistory extends Component {
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
            <div className="OrderHistory">
                <Sidebar />
                <div>
                    <div className="nav_mw">
                    </div>
                    <div className="conteiner_mw">
                        <UserHistoryTable />
                    </div>
                </div>
            </div>
        );
    }
}
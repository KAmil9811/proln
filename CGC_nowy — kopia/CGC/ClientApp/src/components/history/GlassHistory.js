import React, { Component } from 'react';
import { GlassHistoryTable } from './GlassHistoryTable';


export class GlassHistory extends Component {
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
                   
                </div>
                <div className="conteiner_mw">
                    
                    <GlassHistoryTable />
                </div>
            </div>
        );
    }
}
import React, { Component } from 'react';
import { SavedOrdersTable } from './SavedOrdersTable';
import Sidebar from '../Sidebar';
import './SelectionOfOrders.css'



export class SavedProjects extends Component {
    displayName = SavedProjects.name;
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
        if (sessionStorage.getItem('valid') === '') {
            return (
                <div className="HomePage">
                    <h1>Log in to have access!</h1>
                    <button type="submit" className="success_login" onClick={this.goback} >Log in</button>
                </div>
            );
        }
        else {
            return (
                <div className="SavedProject">
                    <Sidebar />
                    <div className="title">
                        <h1 className="titletext">Saved projects</h1>
                    </div>
                    <div className="SavedProject_c">


                        <SavedOrdersTable />

                    </div>


                </div>
            );
        }
    }
}

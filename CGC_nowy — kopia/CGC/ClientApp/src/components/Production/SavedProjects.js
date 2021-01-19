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
        return (
            <div className="SavedProject">
                <Sidebar />
                <div className="save_project_conteiner">


                    <SavedOrdersTable />

                </div>


            </div>
        );
    }
}

import React, { Component } from 'react';
import './Control2.css';
import { UsersTable } from './UsersTable';
import * as FiIcons from 'react-icons/fi';
import Sidebar from '../Sidebar';
import ClipLoader from "react-spinners/ClipLoader";

import { MDBDataTableV5 } from 'mdbreact';
import { Link } from 'react-router-dom';



export class ControlPanel2 extends Component {
    displayName = ControlPanel2.name;
    constructor(props) {
        super(props);
        this.state = {
            table333: {
                columns: [],
                 rows: []
            },
            value: '',
            isLoading: true,

        }
    }
    ///////////////////////TABLICA USERÓW

    componentDidMount() {
        var table = [];
        var link = '';
        const receiver = {
            user: {
                company: sessionStorage.getItem('company'),
            },
        }
        const model = {
            client_id: sessionStorage.getItem('token'),
        }
        if (sessionStorage.getItem('manager') === 'true') {
            link = 'api/Users/Return_All_Users'
        }
        else if (sessionStorage.getItem('superAdmin') === 'true') {
            link = 'api/Users/Return_All_SuperAdmin'
        }
        else {
            link = 'api/Users/Return_All_Admin'
        }
        fetch(`${link}`, {
            method: "post",
            body: JSON.stringify(
                receiver
            ),
            headers: {
                'Authorization': 'Bearer ' + sessionStorage.getItem('token'),
                'Content-Type': 'application/json',
                'Accept': 'application/json',
            }
        })
            .then(res => res.json())
            .then(json => {
                for (var i = 0; i < json.length; i++) {
                    var deleted = '';
                    if (json[i].deleted === false) {
                        deleted = 'Active'
                    }
                    else {
                        deleted = 'Deleted'
                    }
                    table.push({
                        id: i + 1,
                        name: json[i].name,
                        login: json[i].login,
                        password: json[i].password,
                        secondName: json[i].surname,
                        email: json[i].email,
                        permissions: 'Employee',
                        deleted: deleted,
                        action: <Link to="/user_change"><button className="info_t" id={i}
                            onClick={
                                (e) => {

                                    sessionStorage.setItem('editPerm', table[e.target.id].permissions);
                                    sessionStorage.setItem('editLogin', table[e.target.id].login);
                                    sessionStorage.setItem('editPassword', table[e.target.id].password);
                                    sessionStorage.setItem('editName', table[e.target.id].name);
                                    sessionStorage.setItem('editSecondName', table[e.target.id].secondName);
                                    sessionStorage.setItem('editMail', table[e.target.id].email);
                                }
                            }>Edit</button></Link>,
                        del: <button className="danger_t" id={i} onClick={(e) => { this.delete(table[e.target.id].login, table[e.target.id].deleted) }}> Delete/Restore  </button>
                    })
                };

                for (var k = 0; k < table.length; k++) {
                    if (json[k].admin === true) {
                        table[k].permissions = table[k].permissions + ', ' + 'Admin'
                    }
                    if (json[k].super_Admin === true) {
                        table[k].permissions = table[k].permissions + ', ' + 'Super admin'
                    }
                    if (json[k].manager === true) {
                        table[k].permissions = table[k].permissions + ', ' + 'Manager'
                    }
                    if (json[k].magazine_management === true) {
                        table[k].permissions = table[k].permissions + ', ' + 'Magazine management'
                    }
                    if (json[k].machine_management === true) {
                        table[k].permissions = table[k].permissions + ', ' + 'Machine management'
                    }
                    if (json[k].order_management === true) {
                        table[k].permissions = table[k].permissions + ', ' + 'Order management'
                    }
                    if (json[k].cut_management === true) {
                        table[k].permissions = table[k].permissions + ', ' + 'Cut management'
                    }
                }
                this.setState({
                    table333: {
                        columns: [
                            {
                                label: 'Login',
                                field: 'login',
                                sort: 'asc',
                                width: 150
                            },
                            {
                                label: 'Name',
                                field: 'name',
                                sort: 'asc',
                                width: 250
                            },
                            {
                                label: 'Surname',
                                field: 'secondName',
                                sort: 'asc',
                                width: 200
                            },
                            {
                                label: 'Permissions',
                                field: 'permissions',
                                sort: 'asc',
                                width: 100
                            },
                            {
                                label: 'Edit',
                                field: 'action',
                                width: 100
                            },
                            {
                                label: 'Account status',
                                field: 'deleted',
                                width: 100
                            },
                            {
                                label: 'Delete',
                                field: 'del',
                                width: 100
                            }
                        ],
                        rows: table
                    }
                });
                this.setState({
                    isLoading: false
                })
            })
    };

    delete(user, deleted) {
        const receiver = {
            user: {
                login: user
            },
            admin: {
                company: sessionStorage.getItem('company'),
                login: sessionStorage.getItem('login')
            }
        }

        if (deleted === 'Active') {
            fetch(`api/Users/Remove_User_Admin`, {
                method: "post",
                body: JSON.stringify(receiver),
                headers: {
                    'Authorization': 'Bearer ' + sessionStorage.getItem('token'),
                    'Content-Type': 'application/json'
                }
            })

                .then(res => res.json())
                .then(json => {

                    return (json);
                })
                .then(json => {
                    alert("You deleted user")
                })
                .then(json => {
                    window.location.reload();
                })
        }
        else {
            fetch(`api/Users/Restore_User_Admin`, {
                method: "post",
                body: JSON.stringify(receiver),
                headers: {
                    'Authorization': 'Bearer ' + sessionStorage.getItem('token'),
                    'Content-Type': 'application/json'
                }
            })
                .then(res => res.json())
                .then(json => {

                    return (json);
                })
                .then(json => {
                    alert("You activated user")
                })
                .then(json => {
                    window.location.reload();
                })
        }
    }

    table() {
        return (
            <MDBDataTableV5
                data={this.state.table333}
                hover
                entriesOptions={[10, 20, 50, 100]}
                entries={10}
                pagesAmount={10}
                searchTop
                materialSearch
                searchBottom={false}
                responsive
                bordered
                paginationLabel={["Previous", "Next"]}
                sortable
                // small
                theadTextWhite
                theadTextWhite
                className="table_corection_add_o"
            />
        )
    }

    /////////////////////////////////////
    
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
    goback = (event) => {
        this.props.history.push('/')
    }
    goback2 = (event) => {
        this.props.history.push('/home')
    }


    homePage = (event) => {
        this.props.history.push('/home')
    }

    addUser = (event) => {
        this.props.history.push('/add_acount')
    }

    changePassword = (event) => {
        this.props.history.push('/change_password')
    }
    changeEmail = (event) => {
        this.props.history.push('/change_email')
    }

    userChanging = (event) => {
        this.props.history.push('/user_change')
    }

    tableRender() {
        if (sessionStorage.getItem('admin') === 'true' || sessionStorage.getItem('superAdmin') === 'true' || sessionStorage.getItem('manager') === 'true') {
            return (<UsersTable />);
        }
    }

    addAcouButton() {
        if (sessionStorage.getItem('admin') === 'true' || sessionStorage.getItem('superAdmin') === 'true' || sessionStorage.getItem('manager') === 'true') {
            return (<button className="success_add_user" onClick={this.addUser}>Add account</button>);
        }
    }




    editMachine = (event) => {
        this.props.history.push('/cutmachineedit')
    }

    editGlassColor = (event) => {
        this.props.history.push('/glassatibutes')
    }

    userHistory = (event) => {
        this.props.history.push('/user_history')
    }

    typeMachineAdd() {
        if (sessionStorage.getItem('admin') === 'true' || sessionStorage.getItem('superAdmin') === 'true' || sessionStorage.getItem('manager') === 'true') {
            return (
                <button type="button" className="prim_type_machine" onClick={this.editMachine}>Machine types</button>
            )
        }
    }
    colorAndTypeGlassEdit() {
        if (sessionStorage.getItem('admin') === 'true' || sessionStorage.getItem('superAdmin') === 'true' || sessionStorage.getItem('manager') === 'true') {
            return (
                <button type="button" className="prim_color_glass_edit" onClick={this.editGlassColor}>Glass administration</button>
            )
        }
    }

    usersHistoryTable() {
        if (sessionStorage.getItem('admin') === 'true' || sessionStorage.getItem('superAdmin') === 'true' || sessionStorage.getItem('manager') === 'true') {
            return (
                <button type="button" className="prim_all_user_history" onClick={this.userHistory}>Users history</button>
            )
        }
    }
    adminPermissionsRender() {
        if (sessionStorage.getItem('admin') === 'true') {
            return (<option>Admin</option>)
        }
    }
    userPermissionsRender() {
        if (sessionStorage.getItem('user') === 'true') {
            return (<option>Empolyee</option>)
        }
    }
    superAdminPermissionsRender() {
        if (sessionStorage.getItem('superAdmin') === 'true') {
            return (<option>Super admin</option>)
        }
    }
    managerPermissionsRender() {
        if (sessionStorage.getItem('manager') === 'true') {
            return (<option>Menedżer</option>)
        }
    }
    magazineManagerPermissionsRender() {
        if (sessionStorage.getItem('magazineManagement') === 'true') {
            return (<option>Magazynier</option>)
        }
    }
    machineManagerPermissionsRender() {
        if (sessionStorage.getItem('machineManagement') === 'true') {
            return (<option>Menedżer maszyn</option>)
        }
    }
    orderManagerPermissionsRender() {
        if (sessionStorage.getItem('orderManagement') === 'true') {
            return (<option>Menedżer zleceń</option>)
        }
    }
    cutManagerPermissionsRender() {
        if (sessionStorage.getItem('cutManagement') === 'true') {
            return (<option>Menedżer cięcia</option>)
        }
    }



    render() {
        let xd = this.table();
        let buttonAdd = this.addAcouButton();
        /*let xd = this.tableRender();*/
        let typeMachine = this.typeMachineAdd();
        let admin = this.adminPermissionsRender();
        let user = this.userPermissionsRender();
        let superAdmin = this.superAdminPermissionsRender();
        let manager = this.managerPermissionsRender();
        let magazineManagement = this.magazineManagerPermissionsRender();
        let orderManagement = this.orderManagerPermissionsRender();
        let machineManagement = this.machineManagerPermissionsRender();
        let cutManagement = this.cutManagerPermissionsRender();
        let colorGlassEdit = this.colorAndTypeGlassEdit();
        let userHistoryTable = this.usersHistoryTable();
        if (sessionStorage.getItem('valid') === '') {
            return (
                <div className="HomePageFail">
                    <h1>Log in to have access!</h1>
                    <button type="submit" className="success_login2" onClick={this.goback} >Logowanie</button>
                </div>
            );
        }
        else if (sessionStorage.getItem('machineMenagment') === 'true' || sessionStorage.getItem('superAdmin') === 'true' || sessionStorage.getItem('manager') === 'true' || sessionStorage.getItem('admin') === 'true') {
            if (this.state.isLoading === true) {
                return (
                    <div className="Loading">
                        <ClipLoader loading={this.state.isLoading} size={150} />
                        <h1>Loading...</h1>
                    </div>
                )
            }
            else {
                return (

                    <div className="ControlPanel" >


                        <Sidebar />
                        <div className="onteiner_cp2">
                            <div className="title">
                                <h1 className="titletext">Control panel</h1>
                            </div>
                            <div className="conteiner_cp">
                                <div className="control_b">
                                    {typeMachine}
                                    {colorGlassEdit}
                                    {userHistoryTable}
                                    {buttonAdd}
                                </div>
                                <div className="controlpaneltable">
                                    {xd}
                                </div>
                            </div>
                        </div>
                    </div>

                )
            }
        }
        else {
            return (
                <div className="HomePageFail">
                    <h1>Check if you have perrmission to this panel</h1>
                    <button type="submit" className="success_login2" onClick={this.goback2} >Back to home page</button>
                </div>
                
                )
        }
    }


}
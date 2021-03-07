import React, { Component } from 'react';
import { MDBDataTableV5 } from 'mdbreact';
import { Link } from 'react-router-dom';
import './cutMachineEdit.css';
import Sidebar from '../Sidebar';

export class CutMachineEdit extends Component {
    constructor(props) {
        super(props);
        this.state = {
            table: {
                columns: [],
                rows: []
            },
        };
    }

    componentDidMount() {
        var table2 = [];
        const receiver = {
            user: {
                company: sessionStorage.getItem('company'),
            }
        }
        fetch(`api/Machine/Return_All_Type`, {
            method: "post",
            body: JSON.stringify(receiver),
            headers: {
                'Authorization': 'Bearer ' + sessionStorage.getItem('token'),
                'Content-Type': 'application/json'
            }
        })
            .then(res => res.json())
            .then(json => {
               
                for (var i = 0; i < json.length; i++) {
                    table2.push({
                        number: i + 1,
                        type: json[i],
                        edit:
                            <Link to="/machinetypeedit"><button className="info_t" id={i}
                                onClick={
                                    (e) => {
                                        
                                        sessionStorage.setItem('machinetype', json[e.target.id]);
                                    }
                                }>Edit</button>
                            </Link>
                    })
                };
                this.setState({
                    table: {
                        columns: [
                            {
                                label: 'No.',
                                field: 'number',
                                sort: 'asc',
                                width: 150
                            },
                            {
                                label: 'Type',
                                field: 'type',
                                sort: 'asc',
                                width: 150
                            },
                            {
                                label: 'Edit',
                                field: 'edit',
                                width: 150
                            },
                        ],
                        rows: table2
                    }
                });
            })
    };

    backToHome = (event) => {
        this.props.history.push('/controlpanel');
    }

    addTypeMachine = (event) => {
        this.props.history.push('/add_typemachine')
    }

    editTypeMachine = (event) => {
        this.props.history.push('/machinetypeedit')
    }

    goback = (event) => {
        this.props.history.push('/')
    }
    goback2 = (event) => {
        this.props.history.push('/home')
    }

    
    table() {
        return (
            <MDBDataTableV5
                data={this.state.table}
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
                className="table_corection"
            />
        )
    }


    render() {
        let typeTable = this.table();
        if (sessionStorage.getItem('valid') === '') {
            return (
                <div className="HomePageFail">
                    <h1>Log in to have access!</h1>
                    <button type="submit" className="success_login2" onClick={this.goback} >Log in</button>
                </div>
            );
        }
        else if (sessionStorage.getItem('machineManagement') === 'true' || sessionStorage.getItem('superAdmin') === 'true' || sessionStorage.getItem('manager') === 'true' || sessionStorage.getItem('admin') === 'true') {
            return (
                <div className="cutMEdit">
                    <Sidebar />
                    <div className="title">
                        <h1 className="titletext">Machine types</h1>
                    </div>
                    <div className="cut_machine_edit">
                        <div className="nav_machine_e">
                        </div>
                        <div className="conceiner_machine_e">
                            <button type="button" className="success_cm_edit" onClick={this.addTypeMachine}>Add type</button>
                            {typeTable}
                        </div>
                    </div>
                </div>
            )
        }
        else {
            return (
                <div className="HomePageFail">
                    <h1>Check if you have perrmission to this panel</h1>
                    <button type="submit" className="success_login2" onClick={this.goback2} >Back to home page</button>
                </div>
            );

        }
    }


}
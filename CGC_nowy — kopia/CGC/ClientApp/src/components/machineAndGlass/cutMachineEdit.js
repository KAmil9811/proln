import React, { Component } from 'react';
import { MDBDataTable } from 'mdbreact';
import { Link } from 'react-router-dom';
import './cutMachineEdit.css';

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
        fetch(`api/Machine/Return_All_Type`, {
            headers: {
                'Content-Type': 'application/json',
                'Accept': 'application/json'
            }
        })
            .then(res => res.json())
            .then(json => {
                console.log(json.length);
                console.log(json);
                for (var i = 0; i < json.length; i++) {
                    table2.push({
                        number: i + 1,
                        type: json[i],
                        edit:
                            <Link to="/machinetypeedit"><button className="machine_edit_but" id={i}
                                onClick={
                                    (e) => {
                                        console.log(e.target.id);
                                        sessionStorage.setItem('machinetype', json[e.target.id]);
                                    }
                                }>Edytuj</button>
                            </Link>
                    })
                };
                this.setState({
                    table: {
                        columns: [
                            {
                                label: 'Nr',
                                field: 'number',
                                sort: 'asc',
                                width: 150
                            },
                            {
                                label: 'Typ',
                                field: 'type',
                                sort: 'asc',
                                width: 150
                            },
                            {
                                label: 'Edycja',
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


    
    table() {
        return (
            <MDBDataTable
                bordered
                small
                data={this.state.table}
            />
        )
    }


    render() {
        let typeTable = this.table();
        return (
            <div className="CutMEdit">
                <div className="nav_machine_e">
                    <button type="button" className="back_type" onClick={this.backToHome}>Powrót</button>
                </div>
                <div className="conceiner_machine_e">
                     <button type="button" className="add_type_m1" onClick={this.addTypeMachine}>Dodaj typ</button>
                    {typeTable}
                </div>
            </div>
        )
    }


}
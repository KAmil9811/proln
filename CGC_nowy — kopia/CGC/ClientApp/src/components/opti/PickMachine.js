import React, { Component } from "react";
import Sidebar from '../Sidebar';
import './PickMachine.css'

export class PickMachine extends Component {
    displayName = PickMachine;
    constructor(props) {
        super(props);
        this.state = {
            value: '',
            machine: [],
        }
    }

    
    componentDidMount() {
        var table2 = [];
        fetch(`api/Cut/Return_Machine_To_Cut`, {
            headers: {
                'Content-Type': 'application/json',
                'Accept': 'application/json'
            }
        })
            .then(res => res.json())
            .then(json => {
                console.log(json);
                for (var i = 0; i < json.length; i++) {
                    table2.push({
                        color: json[i].no,
                        type: json[i].type,
                    })
                };
                this.setState({
                    machine: table2

                });
            })
    }

    machineSelector = (event) => {
        var tab = []
        for (var i = 0; i < this.state.machine.length; i++) {

            tab.push(< option value={this.state.machine[i].color}> {this.state.machine[i].color + ', ' + this.state.machine[i].type} </option >)


        }
        return (tab)
    }

    cancelAdding = (event) => {
        this.props.history.push('/test')
    }

    saveProject = (event) => {
        event.preventDefault();
        const receiver = {
            machines: {
                no: this.refs.type.value
            },
            cut_Project: {
                Cut_id: sessionStorage.getItem('id_order')
            }

        }


        fetch(`api/Cut/Start_Production`, {
            method: "post",
            body: JSON.stringify(receiver),
            headers: {
                'Content-Type': 'application/json'
            }
        })
            .then(res => res.json())
            /*.then(json => {
                console.log(receiver)
                console.log(json)
                return (json)
            })*/
        this.props.history.push('/glasswarehouse');

        console.log(receiver)

    }

    render() {
        let x = this.machineSelector()
        if (sessionStorage.getItem('valid') === '') {
            return (
                <div className="HomePage">
                    <h1>Zaloguj się, aby usyskać dostęp!</h1>
                    <button type="submit" className="success_login" onClick={this.goback} >Logowanie</button>
                </div>
            );
        }
        else {
            return (

                <div>
                    <Sidebar />
                    <div className="AddOrder1">
                        <form>
                            <div className="form-group">
                                <label>Wybierz maszynę:</label>
                                <select ref="type" type="text" className="form-control">
                                    {x}
                                </select>
                            </div>
                            <div className="form-group">
                                <button type="button" className="danger_pick_machine" onClick={this.cancelAdding}>Anuluj</button>
                                <button type="button" className="success_pick_machine" onClick={this.saveProject}>Wytnij</button>

                            </div>
                        </form>
                    </div>

                </div>
            )
        }
    }

}
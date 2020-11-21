import { action, computed, configure, makeObservable, observable, runInAction } from 'mobx';
import { createContext, SyntheticEvent } from 'react';
import activitiesService from '../api/agent';
import { IActivity } from './../models/activity';

configure({ enforceActions: 'always' });

class ActivityStore {
  constructor() {
    makeObservable(this);
  }

  @observable activitiesRegistry = new Map<string, IActivity>();
  @observable activity: IActivity | undefined;
  @observable loadingInitial = false;
  @observable submittingForm = false;
  @observable target = '';

  @computed get activitiesSortedByDate() {
    return this.activitiesGroupedByDate(Array.from(this.activitiesRegistry.values()));
  }

  activitiesGroupedByDate(activities: IActivity[]) {
    const sortedActivities = activities.sort((a, b) => Date.parse(a.date) - Date.parse(b.date));

    return Object.entries(
      sortedActivities.reduce((activities, activity) => {
        const dateTime = activity.date.split('T')[0];
        activities[dateTime] = activities[dateTime]
          ? [...activities[dateTime], activity]
          : [activity];

        return activities;
      }, {} as { [key: string]: IActivity[] })
    );
  }

  @action loadActivities = async () => {
    this.loadingInitial = true;

    try {
      const activities = await activitiesService.listAll();

      runInAction(() => {
        activities.forEach((activity: IActivity) => {
          activity.date = activity.date.split('.')[0];
          this.activitiesRegistry.set(activity.id, activity);
        });

        this.loadingInitial = false;
      });
    } catch (error) {
      runInAction(() => {
        this.loadingInitial = false;
      });

      console.log(error);
    }
  };

  @action loadActivity = async (id: string) => {
    let activity = this.getActivity(id);

    if (activity) {
      this.activity = activity;
    } else {
      this.loadingInitial = true;

      try {
        activity = await activitiesService.getOne(id);

        runInAction(() => {
          this.activity = activity;
          this.loadingInitial = false;
        });
      } catch (error) {
        runInAction(() => {
          this.loadingInitial = false;
        });

        console.log(error);
      }
    }
  };

  @action clearActivity = () => {
    this.activity = undefined;
  };

  @action createActivity = async (activity: IActivity) => {
    this.submittingForm = true;
    try {
      await activitiesService.create(activity);

      runInAction(() => {
        this.activitiesRegistry.set(activity.id, activity);
        this.submittingForm = false;
      });
    } catch (error) {
      runInAction(() => {
        this.submittingForm = false;
      });

      console.log(error);
    }
  };

  @action updateActivity = async (activity: IActivity) => {
    this.submittingForm = true;

    try {
      await activitiesService.update(activity);

      runInAction(() => {
        this.activitiesRegistry.set(activity.id, activity);
        this.activity = activity;
        this.submittingForm = false;
      });
    } catch (error) {
      runInAction(() => {
        this.submittingForm = false;
      });

      console.log(error);
    }
  };

  @action deleteActivity = async (event: SyntheticEvent<HTMLButtonElement>, id: string) => {
    runInAction(() => {
      this.submittingForm = true;
      this.target = event.currentTarget.name;
    });

    try {
      await activitiesService.delete(id);

      runInAction(() => {
        this.activitiesRegistry.delete(id);
        this.submittingForm = false;
        this.target = '';
      });
    } catch (error) {
      runInAction(() => {
        this.submittingForm = false;
        this.target = '';
      });

      console.log(error);
    }
  };

  getActivity = (id: string) => this.activitiesRegistry.get(id);
}

export default createContext(new ActivityStore());
